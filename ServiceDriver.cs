using System;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    public interface ServiceDriver : IDisposable
    {
        string DisplayName { get; }

        ServiceControllerStatus Status { get; }

        bool CanStop { get; }
        bool CanPauseAndContinue { get; }

        void Start();

        void Pause();
        void Continue();

        void Stop();

        void Refresh();

        void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout);
    }
}
