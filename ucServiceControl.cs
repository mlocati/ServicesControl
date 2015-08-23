using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.ServiceProcess;

namespace MLocati.ServicesControl
{
    public partial class ucServiceControl : UserControl
    {
        private bool IsRestarting;
        public bool Active
        {
            get
            { return this.tmrUpdate.Enabled; }
            set
            { this.tmrUpdate.Enabled = value; }
        }
        private readonly ServiceController Controller;
        public ucServiceControl(ServiceController controller)
        {
            InitializeComponent();
            this.IsRestarting = false;
            this.Controller = controller;
            this.lblServiceName.Text = this.Controller.DisplayName;
            this.RefreshStatus();
            this.Active = true;
        }
        public void RefreshStatus()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.RefreshStatus), null);
                return;
            }
            this.Controller.Refresh();
            string statusText = null;
            Dictionary<Button, bool> okb = new Dictionary<Button, bool>();
            okb.Add(this.btnStart, false);
            okb.Add(this.btnStop, false);
            okb.Add(this.btnPause, false);
            okb.Add(this.btnRestart, false);
            ServiceControllerStatus? status;
            try
            {
                status = this.Controller.Status;
            }
            catch (Exception x)
            {
                status = null;
                statusText = x.Message;
            }
            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case ServiceControllerStatus.ContinuePending:
                        statusText = "Resuming...";
                        break;
                    case ServiceControllerStatus.Paused:
                        statusText = "Paused";
                        okb[this.btnStop] = okb[this.btnStart] = true;
                        break;
                    case ServiceControllerStatus.PausePending:
                        statusText = "Pause pending...";
                        break;
                    case ServiceControllerStatus.Running:
                        statusText = "Running";
                        okb[this.btnRestart] = okb[this.btnStop] = this.Controller.CanStop;
                        okb[this.btnPause] = this.Controller.CanPauseAndContinue;
                        break;
                    case ServiceControllerStatus.StartPending:
                        statusText = "Starting...";
                        break;
                    case ServiceControllerStatus.Stopped:
                        statusText = "Stopped";
                        okb[this.btnStart] = true;
                        break;
                    case ServiceControllerStatus.StopPending:
                        statusText = "Stopping...";
                        break;
                }
                if (statusText == null)
                {
                    statusText = "unknown";
                }
            }
            if (this.lblServiceStatus.Text != statusText)
            {
                this.lblServiceStatus.Text = statusText;
                this.lblServiceStatus.Refresh();
            }
            foreach (KeyValuePair<Button, bool> ob in okb)
            {
                if (ob.Key.Enabled != ob.Value)
                    ob.Key.Enabled = ob.Value;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.DoAction(new ServiceControllerStatus[] { ServiceControllerStatus.Stopped, ServiceControllerStatus.Paused }, new Action[] { this.Controller.Start, this.Controller.Continue });
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.DoAction(new ServiceControllerStatus[] { ServiceControllerStatus.Running, ServiceControllerStatus.Paused }, this.Controller.Stop);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            this.DoAction(ServiceControllerStatus.Running, this.Controller.Pause);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            this.DoAction(ServiceControllerStatus.Running, this.Controller.Stop, true);
        }

        private void DoAction(ServiceControllerStatus status, Action action)
        {
            this.DoAction(new ServiceControllerStatus[] { status }, action, false);
        }
        private void DoAction(ServiceControllerStatus[] statuses, Action action)
        {
            this.DoAction(statuses, new Action[] { action }, false);
        }
        private void DoAction(ServiceControllerStatus[] statuses, Action[] actions)
        {
            this.DoAction(statuses, actions, false);
        }
        private void DoAction(ServiceControllerStatus status, Action action, bool isRestarting)
        {
            this.DoAction(new ServiceControllerStatus[] { status }, new Action[] { action }, isRestarting);
        }
        private void DoAction(ServiceControllerStatus[] statuses, Action action, bool isRestarting)
        {
            this.DoAction(statuses, new Action[] { action }, isRestarting);
        }

        private void DoAction(ServiceControllerStatus[] statuses, Action[] actions, bool isRestarting)
        {
            if (this.bgwActioner.IsBusy) return;
            this.IsRestarting = isRestarting;
            this.Controller.Refresh();
            for (int i = 0; i < statuses.Length; i++)
            {
                if (statuses[i] == this.Controller.Status)
                {
                    try
                    {
                        this.bgwActioner.RunWorkerAsync((actions.Length == 1) ? actions[0] : actions[i]);
                    }
                    catch (Exception x)
                    {
                        this.bgwActioner_RunWorkerCompleted(null, new RunWorkerCompletedEventArgs(null, x, false));
                    }
                    break;
                }
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            this.RefreshStatus();
        }


        private void bgwActioner_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ((Action)e.Argument)();
                e.Result = null;
            }
            catch (Exception x)
            {
                e.Result = x;
            }
        }

        private void bgwActioner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Exception x;
            if ((x = e.Error) == null)
                x = e.Result as Exception;
            if (x != null)
            {
                MessageBox.Show(x.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (this.IsRestarting)
                {
                    this.RefreshStatus();
                    this.Controller.Refresh();
                    if (this.Controller.Status == ServiceControllerStatus.StopPending)
                    {
                        try
                        { this.Controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10)); }
                        catch
                        { }
                        this.Controller.Refresh();
                    }
                    this.DoAction(ServiceControllerStatus.Stopped, this.Controller.Start);
                    return;
                }
            }
        }
    }
}
