using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    class ServiceLikeServiceDriver : ServiceDriver
    {
        abstract class StdOutErr
        {
            public readonly string Chunk;
            protected StdOutErr(string chunk)
            {
                this.Chunk = chunk;
            }
        }
        class StdOut : StdOutErr
        {
            public StdOut(string chunk) : base(chunk) { }
        }
        class StdErr : StdOutErr
        {
            public StdErr(string chunk) : base(chunk) { }

        }
        private List<StdOutErr> _output = new List<StdOutErr>();

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
            process.Start();
            this.Process = process;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this._output.Add(new StdOut(e.Data));
        }
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this._output.Add(new StdErr(e.Data));
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
                }
                if (!this.Process.HasExited)
                {
                    this.Process.Kill();
                }
            }
            this.Process.Dispose();
            this.Process = null;
        }
    }
}
