using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    static class Program
    {
        public static List<ServiceController> Services;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (UAC.OperatingSystemSupport && !UAC.IsElevated)
            {
                try
                {
                    UAC.RunElevated(Application.ExecutablePath, args);
                }
                catch (Win32Exception x)
                {
                    if (x.ErrorCode != -2147467259)
                    {
                        throw;
                    }
                }
                return;
            }
            Program.ReloadServices();
            if (Program.Services.Count == 0)
            {
                foreach (ServiceController sc in ServiceController.GetServices())
                {
                    if (sc.DisplayName.Contains("Apache"))
                    {
                        Program.Services.Add(sc);
                    }
                }
                using (frmSetServices f = new frmSetServices())
                {
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.ShowDialog();
                    if (Program.Services.Count == 0)
                    {
                        return;
                    }
                }
                return;
            }
            using (frmMain frm = new frmMain())
            {
                Application.Run(frm);
            }
            foreach (ServiceController scd in Program.Services)
            {
                try
                { scd.Dispose(); }
                catch
                { }
            }
        }

        public static string ConfigFileName
        {
            get
            {
                return Path.ChangeExtension(Application.ExecutablePath, ".txt");
            }
        }

        public static void ReloadServices()
        {
            Program.Services = new List<ServiceController>();
            string configFilename = Program.ConfigFileName;
            if (File.Exists(configFilename))
            {
                using (FileStream configStream = new FileStream(configFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader configReader = new StreamReader(configStream, true))
                    {
                        string line;
                        while ((line = configReader.ReadLine()) != null)
                        {
                            string[] possibleNames = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (possibleNames.Length > 0)
                            {
                                ServiceController sc = WindowsServices.GetServiceControl(possibleNames);
                                if (sc != null)
                                    Program.Services.Add(sc);
                            }
                        }
                    }
                }
            }

        }
    }
}
