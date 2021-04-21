using System;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    class SystemServiceDriver : ServiceDriver
    {
        public ProgramOutput Output => null;

        public string DisplayName => this.ServiceController.DisplayName;

        public ServiceControllerStatus Status => this.ServiceController.Status;

        public bool CanStop => this.ServiceController.CanStop;

        public bool CanPauseAndContinue => this.ServiceController.CanPauseAndContinue;

        private ServiceController ServiceController;

        public SystemServiceDriver(ServiceController serviceController)
        {
            this.ServiceController = serviceController;
        }

        private string QuotedServiceName
        {
            get
            {
                var result = this.ServiceController.ServiceName.Replace("^", "^^").Replace("\"", "^\"");
                if (result.IndexOf(' ') >= 0)
                {
                    result = $"\"{result}\"";
                }

                return result;
            }
        }
        public void Start()
        {
            Program.SystemServiceTriggerInvoker.run($"start {this.QuotedServiceName}");
        }

        public void Pause()
        {
            Program.SystemServiceTriggerInvoker.run($"pause {this.QuotedServiceName}");
        }

        public void Continue()
        {
            Program.SystemServiceTriggerInvoker.run($"continue {this.QuotedServiceName}");
        }

        public void Stop()
        {
            Program.SystemServiceTriggerInvoker.run($"stop {this.QuotedServiceName}");
        }

        public void Refresh()
        {
            this.ServiceController.Refresh();
        }

        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
        {
            this.ServiceController.WaitForStatus(desiredStatus, timeout);
        }

        public void Dispose()
        {
            if (this.ServiceController != null)
            {
                this.ServiceController.Dispose();
                this.ServiceController = null;
            }
        }
    }
}
