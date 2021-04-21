using System;
using System.IO.Pipes;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace MLocati.ServicesControl
{
    class SystemServiceTrigger : Piper, IDisposable
    {
        private NamedPipeClientStream pipe = null;
        public SystemServiceTrigger(int callerProcessID)
        {
            try
            {
                this.pipe = new NamedPipeClientStream(".", $"ServicesControl-{callerProcessID}", PipeDirection.InOut);
                this.pipe.Connect(500);
            }
            catch
            {
                this.Dispose();
                throw;
            }
        }
        public void Dispose()
        {
            if (this.pipe != null)
            {
                this.pipe.Dispose();
                this.pipe = null;
            }
        }
        public void run()
        {
            var extractCommand = new Regex(@"^(?<command>\w+)(\s+(?<arguments>.+?))?\s*$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            for (; ; )
            {
                string error = null;
                try
                {
                    var line = this.Read(this.pipe);
                    if (string.IsNullOrEmpty(line))
                    {
                        throw new Exception("Empty line");
                    }
                    var match = extractCommand.Match(line);
                    if (!match.Success)
                    {
                        throw new Exception($"Unrecognized line: {line}");
                    }
                    switch (match.Groups["command"].Value.ToLower())
                    {
                        case "start":
                            this.GetSystemServiceController(match.Groups["arguments"]?.Value).Start();
                            break;
                        case "pause":
                            this.GetSystemServiceController(match.Groups["arguments"]?.Value).Pause();
                            break;
                        case "continue":
                            this.GetSystemServiceController(match.Groups["arguments"]?.Value).Continue();
                            break;
                        case "stop":
                            this.GetSystemServiceController(match.Groups["arguments"]?.Value).Stop();
                            break;
                        case "quit":
                            return;
                        default:
                            throw new Exception($"Unrecognized command: {match.Groups["command"].Value}");
                    }
                }
                catch (Exception x)
                {
                    error = x.Message;
                }
                if (error != null)
                {
                    this.Write(this.pipe, $"E: {error}");
                }
                this.Write(this.pipe, "Ready.");
            }
        }


        private ServiceController GetSystemServiceController(string arguments)
        {
            if (!string.IsNullOrEmpty(arguments) && Regex.IsMatch("^\".*\"$", arguments))
            {
                arguments = arguments.Substring(1, arguments.Length - 2);
            }
            if (string.IsNullOrEmpty(arguments))
            {
                throw new Exception("Missing service name");
            }
            foreach (var serviceController in ServiceController.GetServices())
            {
                if (string.Compare(serviceController.ServiceName, arguments, true) == 0)
                {
                    return serviceController;
                }
            }
            throw new Exception($"Unable to find a service named {arguments}");
        }
    }
}
