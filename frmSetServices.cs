using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    public partial class frmSetServices : Form
    {
        public frmSetServices()
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            List<Service> services = new List<Service>();
            foreach (ServiceController sc in ServiceController.GetServices())
            {
                services.Add(new Service(sc));
            }
            services.Sort((a, b) => a.ToString().CompareTo(b.ToString()));
            bool someChecked = false;
            foreach (Service s in services)
            {
                someChecked = someChecked || s.CurrentlyConfigured;
                this.clbServices.Items.Add(s, s.CurrentlyConfigured);
            }
            this.btnOk.Enabled = someChecked;
        }
        private class Service
        {
            private ServiceController _sc;
            private string _text;
            private bool _currentlyConfigured;
            public Service(ServiceController sc)
            {
                this._sc = sc;
                this._text = string.Format("{0} [{1}]", sc.DisplayName, sc.ServiceName);
                //this._currentlyConfigured = (Program.Services != null && Program.Services.Contains(sc));
                this._currentlyConfigured = false;
                if (Program.Services != null)
                {
                    foreach (ServiceController s in Program.Services)
                    {
                        if (s.ServiceName == sc.ServiceName)
                        {
                            this._currentlyConfigured = true;
                            break;
                        }
                    }
                }
            }
            public bool CurrentlyConfigured
            {
                get
                {
                    return this._currentlyConfigured;
                }
            }
            public override string ToString()
            {
                return this._text;
            }
            public ServiceController ServiceController
            {
                get
                {
                    return this._sc;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            StringBuilder sb = null;
            foreach (Service s in this.SelectedServices)
            {
                if (sb == null)
                {
                    sb = new StringBuilder();
                }
                sb.AppendLine(s.ServiceController.ServiceName);
            }
            if (sb == null)
            {
                return;
            }
            System.IO.File.WriteAllText(Program.ConfigFileName, sb.ToString(), Encoding.ASCII);
            Program.ReloadServices();
            this.DialogResult = DialogResult.OK;
        }
        private List<Service> SelectedServices
        {
            get
            {
                List<Service> current = new List<Service>(this.clbServices.CheckedItems.Count);
                foreach (Service s in this.clbServices.CheckedItems)
                {
                    current.Add(s);
                }
                return current;
            }
        }
        private void clbServices_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<Service> current = this.SelectedServices;
            Service item = (Service)this.clbServices.Items[e.Index];
            switch (e.NewValue)
            {
                case CheckState.Checked:
                    if (!current.Contains(item))
                    {
                        current.Add(item);
                    }
                    break;
                case CheckState.Unchecked:
                    if (current.Contains(item))
                    {
                        current.Remove(item);
                    }
                    break;
            }
            this.btnOk.Enabled = current.Count > 0;
        }
    }
}
