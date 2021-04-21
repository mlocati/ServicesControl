using System;
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

        private static SystemServiceTriggerInvoker _systemServiceTriggerInvoker = null;
        public static SystemServiceTriggerInvoker SystemServiceTriggerInvoker
        {
            get
            {
                if (Program._systemServiceTriggerInvoker == null)
                {
                    Program._systemServiceTriggerInvoker = new SystemServiceTriggerInvoker(Application.ExecutablePath);
                }
                return Program._systemServiceTriggerInvoker;
            }
        }

        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length == 2 && string.Compare(args[0], "SystemServiceTrigger", true) == 0)
            {
                var callerProcessID = 0;
                if (int.TryParse(args[1], out callerProcessID))
                {
                    /*
                    while (!System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    System.Diagnostics.Debugger.Break();
                    */
                    if (UAC.OperatingSystemSupport && !UAC.IsElevated)
                    {
                        MessageBox.Show("This app must be executed with elevated privileges.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 1;
                    }
                    try
                    {
                        using (var trigger = new SystemServiceTrigger(callerProcessID))
                        {
                            trigger.run();
                        }
                        return 0;
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 1;
                    }

                }
            }
            try
            {
                Program.ConfigManager = new ServiceConfigManager(Path.ChangeExtension(Application.ExecutablePath, ".txt"));
                if (Program.ConfigManager.ServiceConfigs.Length == 0)
                {
                    using (frmSetServices f = new frmSetServices())
                    {
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.ShowDialog();
                        if (Program.ConfigManager.ServiceConfigs.Length == 0)
                        {
                            return 0;
                        }
                    }
                }
                using (frmMain frm = new frmMain())
                {
                    Application.Run(frm);
                }
            }
            finally
            {
                if (Program._systemServiceTriggerInvoker != null)
                {
                    try
                    {
                        Program._systemServiceTriggerInvoker.Dispose();
                    }
                    catch { }
                    Program._systemServiceTriggerInvoker = null;
                }
            }
            return 0;
        }
    }
}
