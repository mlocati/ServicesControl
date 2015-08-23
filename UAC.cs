using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;

namespace MLocati.ServicesControl
{
    public static class UAC
    {
        public static bool OperatingSystemSupport
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                return (os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6);
            }
        }

        public static bool IsElevated
        {
            get
            {
                WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void RunElevated(string program, string[] arguments)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = program;
            StringBuilder args = null;
            if (arguments != null)
            {
                foreach (string a in arguments)
                {
                    if (string.IsNullOrEmpty(a))
                    {
                        continue;
                    }
                    if (args == null)
                    {
                        args = new StringBuilder();
                    }
                    else
                    {
                        args.Append(' ');
                    }
                    string a2 = a.Replace("^", "^^").Replace("\"", "^\"");
                    if (a2.IndexOf(' ') >= 0)
                    {
                        args.Append('"').Append(a2).Append('"');
                    }
                    else
                    {
                        args.Append(a2);
                    }
                }
            }
            processInfo.Arguments = (args == null) ? "" : args.ToString();
            Process.Start(processInfo);
        }
    }
}
