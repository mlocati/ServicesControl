namespace MLocati.ServicesControl
{
    partial class frmSetServices
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
            this.clbServices = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tpSystem = new System.Windows.Forms.TabPage();
            this.tpCustom = new System.Windows.Forms.TabPage();
            this.lnkAddCustom = new System.Windows.Forms.LinkLabel();
            this.pnlCustom = new System.Windows.Forms.Panel();
            this.tcTabs.SuspendLayout();
            this.tpSystem.SuspendLayout();
            this.tpCustom.SuspendLayout();
            this.SuspendLayout();
            // 
            // clbServices
            // 
            this.clbServices.CheckOnClick = true;
            this.clbServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbServices.FormattingEnabled = true;
            this.clbServices.IntegralHeight = false;
            this.clbServices.Location = new System.Drawing.Point(3, 3);
            this.clbServices.Name = "clbServices";
            this.clbServices.Size = new System.Drawing.Size(356, 283);
            this.clbServices.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(226, 333);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(307, 333);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tcTabs
            // 
            this.tcTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcTabs.Controls.Add(this.tpSystem);
            this.tcTabs.Controls.Add(this.tpCustom);
            this.tcTabs.Location = new System.Drawing.Point(12, 12);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(370, 315);
            this.tcTabs.TabIndex = 0;
            // 
            // tpSystem
            // 
            this.tpSystem.Controls.Add(this.clbServices);
            this.tpSystem.Location = new System.Drawing.Point(4, 22);
            this.tpSystem.Name = "tpSystem";
            this.tpSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSystem.Size = new System.Drawing.Size(362, 289);
            this.tpSystem.TabIndex = 0;
            this.tpSystem.Text = "System services";
            this.tpSystem.UseVisualStyleBackColor = true;
            // 
            // tpCustom
            // 
            this.tpCustom.Controls.Add(this.pnlCustom);
            this.tpCustom.Controls.Add(this.lnkAddCustom);
            this.tpCustom.Location = new System.Drawing.Point(4, 22);
            this.tpCustom.Name = "tpCustom";
            this.tpCustom.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustom.Size = new System.Drawing.Size(362, 289);
            this.tpCustom.TabIndex = 1;
            this.tpCustom.Text = "Custom programs";
            this.tpCustom.UseVisualStyleBackColor = true;
            // 
            // lnkAddCustom
            // 
            this.lnkAddCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAddCustom.AutoSize = true;
            this.lnkAddCustom.Location = new System.Drawing.Point(252, 269);
            this.lnkAddCustom.Name = "lnkAddCustom";
            this.lnkAddCustom.Size = new System.Drawing.Size(104, 13);
            this.lnkAddCustom.TabIndex = 1;
            this.lnkAddCustom.TabStop = true;
            this.lnkAddCustom.Text = "Add custom program";
            this.lnkAddCustom.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddCustom_LinkClicked);
            // 
            // pnlCustom
            // 
            this.pnlCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCustom.AutoScroll = true;
            this.pnlCustom.Location = new System.Drawing.Point(6, 6);
            this.pnlCustom.Name = "pnlCustom";
            this.pnlCustom.Size = new System.Drawing.Size(350, 260);
            this.pnlCustom.TabIndex = 0;
            // 
            // frmSetServices
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 368);
            this.Controls.Add(this.tcTabs);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 115);
            this.Name = "frmSetServices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set services - ServicesControl";
            this.tcTabs.ResumeLayout(false);
            this.tpSystem.ResumeLayout(false);
            this.tpCustom.ResumeLayout(false);
            this.tpCustom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbServices;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tpSystem;
        private System.Windows.Forms.TabPage tpCustom;
        private System.Windows.Forms.LinkLabel lnkAddCustom;
        private System.Windows.Forms.Panel pnlCustom;
    }
}