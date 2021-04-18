using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    public partial class frmMain : Form
    {
        private bool _shown = false;
        private bool _quitting = false;

        public frmMain()
        {
            this.InitializeComponent();
            this.trayIco.Icon = this.Icon = Program.Icon;
            this.BuildServicesList();
        }

        private List<ucServiceControl> getUCControllers()
        {
            var result = new List<ucServiceControl>();
            foreach (var control in this.Controls)
            {
                if (control is ucServiceControl)
                {
                    result.Add((ucServiceControl)control);
                }
            }
            result.Sort((a, b) => a.Top - b.Top);
            return result;
        }

        private void BuildServicesList()
        {
            this.SuspendLayout();
            foreach (var uc in this.getUCControllers())
            {
                this.Controls.Remove(uc);
                uc.Dispose();
            }
            ucServiceControl prev = null;
            int tabIndex = -1;
            ServiceController[] systemServices = null;
            foreach (var config in Program.ConfigManager.ServiceConfigs)
            {
                ServiceDriver driver = null;
                if (config is SystemServiceConfig)
                {
                    if (systemServices == null)
                    {
                        systemServices = ServiceController.GetServices();
                    }
                    foreach (var systemService in systemServices)
                    {
                        if (systemService.ServiceName.Equals(config.ServiceName))
                        {
                            driver = new SystemServiceDriver(systemService);
                            break;
                        }
                    }
                }
                else if (config is ServiceLikeServiceConfig)
                {
                    driver = new ServiceLikeServiceDriver((ServiceLikeServiceConfig)config);
                }
                if (driver == null)
                {
                    continue;
                }
                ucServiceControl cur = new ucServiceControl(driver);
                cur.TabIndex = ++tabIndex;
                if (prev == null)
                {
                    cur.Location = new Point(5, this.tsbQuit.Height + 5);
                    cur.Width = this.Width - 25;
                }
                else
                {
                    cur.Location = new Point(prev.Left, prev.Bottom + 1);
                    cur.Size = prev.Size;
                }
                cur.Visible = true;
                this.Controls.Add(cur);
                prev = cur;
            }
            this.tsTools.TabIndex = ++tabIndex;
            this.SetHeight();
            this.SetPosition();
            this.ResumeLayout();
        }

        private bool _trayed = false;
        private bool Trayed
        {
            get
            {
                return this._trayed;
            }
            set
            {
                if (this._trayed != value)
                {
                    this._trayed = value;
                    var controllers = this.getUCControllers();
                    if (!this._trayed)
                    {
                        foreach (var uc in controllers)
                        {
                            uc.RefreshStatus();
                        }
                    }
                    if (this._trayed)
                    {
                        this.Hide();
                        this.trayIco.Visible = true;
                    }
                    else
                    {
                        this.Show();
                        this.trayIco.Visible = false;
                    }
                    foreach (var uc in controllers)
                    {
                        uc.Active = !this._trayed;
                    }
                }
            }
        }

        private void trayIco_Click(object sender, EventArgs e)
        {
            this.SetPosition();
            this.Trayed = false;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this._shown = true;
            this.SetHeight();
            this.Trayed = true;
        }

        private void SetHeight()
        {
            if (this._shown)
            {
                var controllers = this.getUCControllers();
                this.Height = this.Bounds.Height - this.ClientSize.Height + controllers[controllers.Count - 1].Bottom + 10;
            }
        }
        private void SetPosition()
        {
            if (this._shown)
            {
                Screen screen = Screen.FromControl(this);
                this.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - this.Width;
                this.Top = screen.WorkingArea.Top + screen.WorkingArea.Height - this.Height;
            }
        }
        private void tsbServicesControlManager_Click(object sender, EventArgs e)
        {
            DoShellExecute("services.msc");
        }

        private void tsbEvents_Click(object sender, EventArgs e)
        {
            DoShellExecute("eventvwr.msc");
        }

        private void tsbQuit_Click(object sender, EventArgs e)
        {
            this._quitting = true;
            this.Close();
        }

        private static void DoShellExecute(string what)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = "";
                psi.ErrorDialog = false;
                psi.FileName = what;
                psi.UseShellExecute = true;
                psi.Verb = "Open";
                psi.WindowStyle = ProcessWindowStyle.Normal;
                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    if (this._quitting != true)
                    {
                        this.Trayed = true;
                        e.Cancel = true;
                        return;
                    }
                    break;
            }
            foreach(var controller in this.getUCControllers())
            {
                if (controller.Driver is ServiceLikeServiceDriver)
                {
                    controller.Driver.Stop();
                }
            }
         }

        private void tsbOptions_Click(object sender, EventArgs e)
        {
            using (frmSetServices f = new frmSetServices())
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    this.BuildServicesList();
                }
            }
        }
    }
}
