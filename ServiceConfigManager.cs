using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MLocati.ServicesControl
{
    internal class ServiceConfigManager
    {
        private readonly string Filename;

        private ServiceConfig[] _serviceConfigs;
        public ServiceConfig[] ServiceConfigs
        {
            get { return this._serviceConfigs; }
        }

        public ServiceConfigManager(string filename)
        {
            this.Filename = filename;
            this.Reload();
        }

        public void Reload()
        {
            var serviceConfigs = new List<ServiceConfig>();
            var lines = this.ReadFile();
            var rxAttribs = new Regex(@"^[ \t]+(?<name>(executable|currentdirectory|arguments))[ \t]*:[ \t]*(?<value>.*?)[ \t]*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            for (var lineIndex = 0; lineIndex < lines.Length;)
            {
                var serviceName = lines[lineIndex++];
                if (serviceName != serviceName.Trim())
                {
                    throw new InvalidDataException($"Invalid line {lineIndex}: {serviceName}");
                }
                var attribs = new Dictionary<string, string>();
                for (; lineIndex < lines.Length;)
                {
                    var match = rxAttribs.Match(lines[lineIndex]);
                    if (!match.Success)
                    {
                        break;
                    }
                    var attribName = match.Groups["name"].Value.ToLower();
                    if (attribs.ContainsKey(attribName))
                    {
                        throw new InvalidDataException($"Duplicated attribute: {attribName}");
                    }
                    attribs.Add(attribName, match.Groups["value"].Value);
                    lineIndex++;
                }
                if (attribs.ContainsKey("executable"))
                {
                    serviceConfigs.Add(new ServiceLikeServiceConfig(
                        serviceName,
                        attribs["executable"],
                        attribs.ContainsKey("currentdirectory") ? attribs["currentdirectory"] : "",
                        attribs.ContainsKey("arguments") ? attribs["arguments"] : ""
                    ));
                }
                else
                {
                    serviceConfigs.Add(new SystemServiceConfig(serviceName));
                }
            }
            this._serviceConfigs = serviceConfigs.ToArray();
        }

        public void Save(ServiceConfig[] serviceConfigs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var serviceConfig in serviceConfigs)
            {
                sb.AppendLine(serviceConfig.ServiceName);
                if (serviceConfig is ServiceLikeServiceConfig)
                {
                    var sc = (ServiceLikeServiceConfig)serviceConfig;
                    sb.AppendLine($"\tExecutable: {sc.Executable}");
                    sb.AppendLine($"\tCurrentDirectory: {sc.CurrentDirectory}");
                    sb.AppendLine($"\tArguments: {sc.Arguments}");
                }
            }
            using (var configStream = new FileStream(this.Filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                configStream.SetLength(0L);
                using (var configWriter = new StreamWriter(configStream, Encoding.UTF8))
                {
                    configWriter.Write(sb.ToString());
                    configStream.Flush();
                }
            }
            this._serviceConfigs = serviceConfigs;
        }
        private string[] ReadFile()
        {
            var result = new List<string>();
            if (File.Exists(this.Filename))
            {
                using (var configStream = new FileStream(this.Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var configReader = new StreamReader(configStream, Encoding.UTF8))
                    {
                        string line;
                        while ((line = configReader.ReadLine()) != null)
                        {
                            line = line.TrimEnd();
                            if (line.Length > 0)
                            {
                                result.Add(line);
                            }
                        }
                    }
                }
            }
            return result.ToArray();
        }
    }
}
