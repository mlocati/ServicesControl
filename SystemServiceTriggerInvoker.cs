using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

namespace MLocati.ServicesControl
{
    class SystemServiceTriggerInvoker : Piper, IDisposable
    {
        private NamedPipeServerStream pipe = null;

        private Process process;

        public SystemServiceTriggerInvoker(string executablePath)
        {
            try
            {
                var processID = Process.GetCurrentProcess().Id;
                this.pipe = new NamedPipeServerStream($"ServicesControl-{processID}", PipeDirection.InOut, 1);
                this.process = new Process();
                this.process.StartInfo.CreateNoWindow = false;
                this.process.StartInfo.ErrorDialog = true;
                this.process.StartInfo.FileName = executablePath;
                this.process.StartInfo.Arguments = $"SystemServiceTrigger {processID}";
                this.process.StartInfo.UseShellExecute = true;
                this.process.StartInfo.Verb = "runas";
                this.process.Start();
                if (!this.process.HasExited)
                {
                    this.process.WaitForExit(50);
                }
                if (this.process.HasExited)
                {
                    throw new Exception("Failed to start service trigger");
                }
                this.pipe.WaitForConnection();
            }
            catch
            {
                this.Dispose();
                throw;
            }
        }

        public void run(string command)
        {
            this.Write(this.pipe, command);
            var error = new StringBuilder();
            for (; ; )
            {
                if (this.process != null)
                {
                    this.process.Refresh();
                }
                if (this.process == null || this.process.HasExited)
                {
                    throw new Exception("The system service trigger terminated unexpectedly.");
                }
                var line = this.Read(this.pipe);
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line == "Ready.")
                {
                    break;
                }
                if (line.StartsWith("E: "))
                {
                    error.AppendLine(line.Substring(2));
                }
            }
            if (error.Length > 0)
            {
                throw new Exception(error.ToString());
            }
        }

        public void Dispose()
        {
            if (this.process != null)
            {
                if (!this.process.HasExited)
                {
                    if (this.pipe.IsConnected)
                    {
                        try
                        {
                            this.Write(this.pipe, "quit");
                            this.process.WaitForExit(300);
                        }
                        catch
                        {

                        }
                    }
                    if (!this.process.HasExited)
                    {
                        this.process.Kill();
                    }
                }
                this.process.Dispose();
                this.process = null;
            }
        }
    }
}
