using System;
using System.Security.Principal;

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
    }
}
