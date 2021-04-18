using System;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    class SystemServiceDriver : ServiceDriver
    {
        public string DisplayName => this.ServiceController.DisplayName;

        public ServiceControllerStatus Status => this.ServiceController.Status;

        public bool CanStop => this.ServiceController.CanStop;

        public bool CanPauseAndContinue => this.ServiceController.CanPauseAndContinue;

        private ServiceController ServiceController;

        public SystemServiceDriver(ServiceController serviceController)
        {
            this.ServiceController = serviceController;
        }

        public void Start()
        {
            this.ServiceController.Start();
        }

        public void Pause()
        {
            this.ServiceController.Pause();
        }

        public void Continue()
        {
            this.ServiceController.Continue();
        }

        public void Stop()
        {
            this.ServiceController.Stop();
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
