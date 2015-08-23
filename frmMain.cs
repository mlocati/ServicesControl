using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    public partial class frmMain : Form
    {
        private List<ucServiceControl> ucControllers;

        private bool _shown = false;
        private bool _quitting = false;

        public frmMain()
        {
            this.InitializeComponent();
            this.trayIco.Icon = this.Icon = Program.Icon;
            this.BuildServicesList();
        }

        private void BuildServicesList()
        {
            this.SuspendLayout();
            List<ucServiceControl> controllers = this.ucControllers;
            this.ucControllers = new List<ucServiceControl>();
            if (controllers != null)
            {
                foreach (ucServiceControl uc in controllers)
                {
                    if (uc != null)
                    {
                        if (this.Controls.Contains(uc))
                        {
                            this.Controls.Remove(uc);
                        }
                        uc.Dispose();
                    }
                }
            }
            ucServiceControl prev = null;
            int tabIndex = -1;
            foreach (ServiceController sc in Program.Services)
            {
                ucServiceControl cur = new ucServiceControl(sc);
                cur.TabIndex = ++tabIndex;
                this.Controls.Add(cur);
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
                this.ucControllers.Add(cur);
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
                    if (!this._trayed)
                    {
                        foreach (ucServiceControl uc in this.ucControllers)
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
                    foreach (ucServiceControl uc in this.ucControllers)
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
                this.Height = this.Bounds.Height - this.ClientSize.Height + this.ucControllers[this.ucControllers.Count - 1].Bottom + 10;
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
                    }
                    break;
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
