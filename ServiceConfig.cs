namespace MLocati.ServicesControl
{
    public abstract class ServiceConfig
    {
        public readonly string ServiceName;
        protected ServiceConfig(string serviceName)
        {
            this.ServiceName = serviceName;
        }
    }

    public class SystemServiceConfig : ServiceConfig
    {
        public SystemServiceConfig(string serviceName)
            : base(serviceName)
        {
        }

    }

    public class ServiceLikeServiceConfig : ServiceConfig
    {
        public readonly string Executable;
        public readonly string CurrentDirectory;
        public readonly string Arguments;
        public ServiceLikeServiceConfig(string serviceName, string executable, string currentDirectory, string arguments)
            : base(serviceName)
        {
            this.Executable = executable;
            this.CurrentDirectory = currentDirectory;
            this.Arguments = arguments;
        }
    }
}
