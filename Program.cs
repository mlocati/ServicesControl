using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    static class Program
    {
        public static ServiceConfigManager ConfigManager;

        private static Icon _icon = null;
        public static Icon Icon
        {
            get
            {
                if (Program._icon == null)
                {
                    Program._icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
                }
                return Program._icon;
            }
        }

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
            Program.ConfigManager = new ServiceConfigManager(Path.ChangeExtension(Application.ExecutablePath, ".txt"));
            if (Program.ConfigManager.ServiceConfigs.Length == 0)
            {
                using (frmSetServices f = new frmSetServices())
                {
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.ShowDialog();
                    if (Program.ConfigManager.ServiceConfigs.Length == 0)
                    {
                        return;
                    }
                }
            }
            using (frmMain frm = new frmMain())
            {
                Application.Run(frm);
            }
        }
    }
}
