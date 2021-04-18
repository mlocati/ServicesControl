using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    public partial class frmSetServices : Form
    {
        public frmSetServices()
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.PopulateSystemServices();
            this.PopulateCustomServices();
        }

        private void PopulateSystemServices()
        {
            var configured = new List<string>();
            foreach (var config in Program.ConfigManager.ServiceConfigs)
            {
                if (config is SystemServiceConfig)
                {
                    configured.Add(config.ServiceName.ToLower());
                }
            }
            var services = ServiceController.GetServices();
            Array.Sort(services, (a, b) => string.Compare(a.ServiceName, b.ServiceName, true));
            foreach (var service in services)
            {
                this.clbServices.Items.Add(service.ServiceName, configured.Contains(service.ServiceName.ToLower()));
            }
        }

        private void PopulateCustomServices()
        {
            foreach (var config in Program.ConfigManager.ServiceConfigs)
            {
                if (config is ServiceLikeServiceConfig)
                {
                    this.AddCustomApp((ServiceLikeServiceConfig)config);
                }
            }
        }
        private void lnkAddCustom_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uc = this.AddCustomApp();
            uc.txtName.Focus();
        }

        private ucServiceLikeConfig AddCustomApp()
        {
            return this.AddCustomApp(null);
        }
        private ucServiceLikeConfig AddCustomApp(ServiceLikeServiceConfig config)
        {
            ucServiceLikeConfig uc;
            var top = 0;
            var tabIndex = 0;
            foreach (var control in this.pnlCustom.Controls)
            {
                if (control is ucServiceLikeConfig)
                {
                    uc = (ucServiceLikeConfig)control;
                    top = Math.Max(top, uc.Bottom);
                    tabIndex = Math.Max(tabIndex, uc.TabIndex + 1);
                }
            }
            uc = new ucServiceLikeConfig(config);
            uc.TabIndex = ++tabIndex;
            this.pnlCustom.Controls.Add(uc);
            uc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uc.Location = new Point(0, top);
            uc.Size = new Size(this.pnlCustom.Width, uc.MinimumSize.Height);
            uc.AskRemove += this.RemoveCustomApp;
            return uc;
        }

        private void RemoveCustomApp(object sender, EventArgs e)
        {
            var uc = sender as ucServiceLikeConfig;
            if (uc == null)
            {
                return;
            }
            var ucAll = new List<ucServiceLikeConfig>();
            foreach (var control in this.pnlCustom.Controls)
            {
                if (control is ucServiceLikeConfig)
                {
                    ucAll.Add((ucServiceLikeConfig)control);
                }
            }
            ucAll.Sort((a, b) => a.Top - b.Top);
            var ucIndex = ucAll.IndexOf(uc);
            if (ucIndex < 0)
            {
                return;
            }
            this.pnlCustom.Controls.Remove(uc);
            for (var index = ucIndex + 1; index < ucAll.Count; index++)
            {
                ucAll[index].Top = ucAll[index - 1].Top;
            }
            uc.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var namesLC = new List<string>();
            var configs = new List<ServiceConfig>();
            foreach (var item in this.clbServices.CheckedItems)
            {
                var serviceName = item as string;
                if (serviceName == null)
                {
                    continue;
                }
                configs.Add(new SystemServiceConfig(serviceName));
                namesLC.Add(serviceName.ToLower());
            }
            foreach (var uc in this.pnlCustom.Controls)
            {
                var ucConfig = uc as ucServiceLikeConfig;
                if (ucConfig == null)
                {
                    continue;
                }
                ServiceLikeServiceConfig config;
                try
                {
                    config = ucConfig.GetConfig();
                }
                catch (ucServiceLikeConfig.ConfigException x)
                {
                    this.tcTabs.SelectedTab = this.tpCustom;
                    x.Control.Focus();
                    MessageBox.Show(this, x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (namesLC.Contains(config.ServiceName.ToLower()))
                {
                    this.tcTabs.SelectedTab = this.tpCustom;
                    ucConfig.txtName.Focus();
                    MessageBox.Show(this, $"The name '{config.ServiceName}' is duplicated: please choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                configs.Add(config);
                namesLC.Add(config.ServiceName.ToLower());
            }
            if (configs.Count == 0)
            {
                MessageBox.Show(this, "Please specify at least one service", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            configs.Sort((a, b) => string.Compare(a.ServiceName, b.ServiceName));
            try
            {
                Program.ConfigManager.Save(configs.ToArray());
            }
            catch (Exception x)
            {
                MessageBox.Show(this, x.Message, "Error saving configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
