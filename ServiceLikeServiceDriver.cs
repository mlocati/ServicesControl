using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    class ServiceLikeServiceDriver : ServiceDriver
    {
        private readonly ProgramOutput _output = new ProgramOutput();
        public ProgramOutput Output => this._output;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        [DllImport("kernel32.dll")]
        private static extern bool GenerateConsoleCtrlEvent(UInt32 dwCtrlEvent, UInt32 dwProcessGroupId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private delegate bool ConsoleCtrlDelegate(UInt32 dwCtrlType);
        private const UInt32 CTRL_C_EVENT = 0;

        private Process Process;
        public string DisplayName => this.Config.ServiceName;

        public ServiceControllerStatus Status
        {
            get
            {
                if (this.Process == null)
                {
                    return ServiceControllerStatus.Stopped;
                }
                return this.Process.HasExited ? ServiceControllerStatus.Stopped : ServiceControllerStatus.Running;
            }
        }

        public bool CanStop => this.Process != null && !this.Process.HasExited;

        public bool CanPauseAndContinue => false;

        private readonly ServiceLikeServiceConfig Config;
        public ServiceLikeServiceDriver(ServiceLikeServiceConfig config)
        {
            this.Config = config;
        }

        public void Start()
        {
            this.StopProcess();
            this._output.Clear();
            var process = new Process();
            process.StartInfo.FileName = this.Config.Executable;
            process.StartInfo.Arguments = this.Config.Arguments;
            process.StartInfo.WorkingDirectory = this.Config.CurrentDirectory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            using (var impersonationContext = WindowsIdentity.GetCurrent().Impersonate())
            {
                process.Start();
            }
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            this.Process = process;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                this.Output.Add(e.Data, ProgramOutput.Type.StdOut);
            }
        }
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                this.Output.Add(e.Data, ProgramOutput.Type.StdErr);
            }
        }

        public void Pause()
        {
            throw new InvalidOperationException();
        }

        public void Continue()
        {
            throw new InvalidOperationException();
        }

        public void Stop()
        {
            this.StopProcess();
        }

        public void Refresh()
        {
            if (this.Process != null)
            {
                this.Process.Refresh();
            }
        }

        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
        {
            throw new InvalidOperationException();
        }


        public void Dispose()
        {
            this.StopProcess();
        }

        private void StopProcess()
        {
            if (this.Process == null)
            {
                return;
            }
            this.Process.Refresh();
            if (!this.Process.HasExited)
            {
                if (AttachConsole((UInt32)this.Process.Id))
                {
                    SetConsoleCtrlHandler(null, true);
                    try
                    {
                        if (GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                        {
                            this.Process.WaitForExit(1000);
                        }
                    }
                    finally
                    {
                        SetConsoleCtrlHandler(null, false);
                        FreeConsole();
                    }
                    this.Process.Refresh();
                }
            }
            if (!this.Process.HasExited)
            {
                if (this.Process.CloseMainWindow())
                {
                    this.Process.WaitForExit(1000);
                    this.Process.Refresh();
                }
            }
            if (!this.Process.HasExited)
            {
                var taskKill = Path.Combine(Environment.SystemDirectory, "taskkill.exe");
                if (File.Exists(taskKill))
                {
                    var killer = new Process();
                    killer.StartInfo.FileName = taskKill;
                    killer.StartInfo.Arguments = $"/PID {this.Process.Id} /T /F";
                    killer.StartInfo.UseShellExecute = false;
                    killer.StartInfo.CreateNoWindow = true;
                    killer.StartInfo.ErrorDialog = false;
                    killer.Start();
                    killer.WaitForExit();
                    this.Process.Refresh();
                }
            }
            if (!this.Process.HasExited)
            {
                this.Process.Kill();
                this.Process.WaitForExit(1000);
                this.Process.Refresh();
            }
            this.Process.Dispose();
            this.Process = null;
        }
    }
}
