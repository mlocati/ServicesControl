namespace MLocati.ServicesControl
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.trayIco = new System.Windows.Forms.NotifyIcon(this.components);
            this.tsTools = new System.Windows.Forms.ToolStrip();
            this.tsbServicesControlManager = new System.Windows.Forms.ToolStripButton();
            this.tsbEvents = new System.Windows.Forms.ToolStripButton();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.tsbQuit = new System.Windows.Forms.ToolStripButton();
            this.tsTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIco
            // 
            this.trayIco.Visible = true;
            this.trayIco.Click += new System.EventHandler(this.trayIco_Click);
            // 
            // tsTools
            // 
            this.tsTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbServicesControlManager,
            this.tsbEvents,
            this.tsbOptions,
            this.tsbQuit});
            this.tsTools.Location = new System.Drawing.Point(0, 0);
            this.tsTools.Name = "tsTools";
            this.tsTools.Size = new System.Drawing.Size(300, 25);
            this.tsTools.TabIndex = 0;
            this.tsTools.Text = "toolStrip1";
            // 
            // tsbServicesControlManager
            // 
            this.tsbServicesControlManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbServicesControlManager.Image = ((System.Drawing.Image)(resources.GetObject("tsbServicesControlManager.Image")));
            this.tsbServicesControlManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbServicesControlManager.Name = "tsbServicesControlManager";
            this.tsbServicesControlManager.Size = new System.Drawing.Size(36, 22);
            this.tsbServicesControlManager.Text = "SCM";
            this.tsbServicesControlManager.ToolTipText = "Open Services Control Manager";
            this.tsbServicesControlManager.Click += new System.EventHandler(this.tsbServicesControlManager_Click);
            // 
            // tsbEvents
            // 
            this.tsbEvents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEvents.Image = ((System.Drawing.Image)(resources.GetObject("tsbEvents.Image")));
            this.tsbEvents.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEvents.Name = "tsbEvents";
            this.tsbEvents.Size = new System.Drawing.Size(45, 22);
            this.tsbEvents.Text = "Events";
            this.tsbEvents.ToolTipText = "Open the Events Viewer";
            this.tsbEvents.Click += new System.EventHandler(this.tsbEvents_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbOptions.Image = ((System.Drawing.Image)(resources.GetObject("tsbOptions.Image")));
            this.tsbOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Size = new System.Drawing.Size(53, 22);
            this.tsbOptions.Text = "Options";
            this.tsbOptions.ToolTipText = "Set the application options";
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // tsbQuit
            // 
            this.tsbQuit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbQuit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbQuit.Image = ((System.Drawing.Image)(resources.GetObject("tsbQuit.Image")));
            this.tsbQuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuit.Name = "tsbQuit";
            this.tsbQuit.Size = new System.Drawing.Size(34, 22);
            this.tsbQuit.Text = "Quit";
            this.tsbQuit.ToolTipText = "Exit the application";
            this.tsbQuit.Click += new System.EventHandler(this.tsbQuit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 261);
            this.Controls.Add(this.tsTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "ServicesControl";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.tsTools.ResumeLayout(false);
            this.tsTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon trayIco;
        private System.Windows.Forms.ToolStrip tsTools;
        private System.Windows.Forms.ToolStripButton tsbQuit;
        private System.Windows.Forms.ToolStripButton tsbServicesControlManager;
        private System.Windows.Forms.ToolStripButton tsbEvents;
        private System.Windows.Forms.ToolStripButton tsbOptions;
    }
}