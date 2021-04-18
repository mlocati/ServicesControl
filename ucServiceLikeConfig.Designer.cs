
namespace MLocati.ServicesControl
{
    partial class ucServiceLikeConfig
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblExecutable = new System.Windows.Forms.Label();
            this.txtExecutable = new System.Windows.Forms.TextBox();
            this.btnExecutable = new System.Windows.Forms.Button();
            this.btnCurrentDirectory = new System.Windows.Forms.Button();
            this.txtCurrentDirectory = new System.Windows.Forms.TextBox();
            this.lblCurrentDirectory = new System.Windows.Forms.Label();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.lblArguments = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblExecutable
            // 
            this.lblExecutable.AutoSize = true;
            this.lblExecutable.Location = new System.Drawing.Point(4, 42);
            this.lblExecutable.Name = "lblExecutable";
            this.lblExecutable.Size = new System.Drawing.Size(60, 13);
            this.lblExecutable.TabIndex = 2;
            this.lblExecutable.Text = "Executable";
            // 
            // txtExecutable
            // 
            this.txtExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecutable.Location = new System.Drawing.Point(4, 59);
            this.txtExecutable.Name = "txtExecutable";
            this.txtExecutable.Size = new System.Drawing.Size(402, 20);
            this.txtExecutable.TabIndex = 3;
            this.txtExecutable.Leave += new System.EventHandler(this.txtExecutable_Leave);
            // 
            // btnExecutable
            // 
            this.btnExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutable.Location = new System.Drawing.Point(412, 57);
            this.btnExecutable.Name = "btnExecutable";
            this.btnExecutable.Size = new System.Drawing.Size(30, 23);
            this.btnExecutable.TabIndex = 4;
            this.btnExecutable.Text = "...";
            this.btnExecutable.UseVisualStyleBackColor = true;
            this.btnExecutable.Click += new System.EventHandler(this.btnExecutable_Click);
            // 
            // btnCurrentDirectory
            // 
            this.btnCurrentDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCurrentDirectory.Location = new System.Drawing.Point(411, 102);
            this.btnCurrentDirectory.Name = "btnCurrentDirectory";
            this.btnCurrentDirectory.Size = new System.Drawing.Size(30, 23);
            this.btnCurrentDirectory.TabIndex = 7;
            this.btnCurrentDirectory.Text = "...";
            this.btnCurrentDirectory.UseVisualStyleBackColor = true;
            this.btnCurrentDirectory.Click += new System.EventHandler(this.btnCurrentDirectory_Click);
            // 
            // txtCurrentDirectory
            // 
            this.txtCurrentDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentDirectory.Location = new System.Drawing.Point(3, 104);
            this.txtCurrentDirectory.Name = "txtCurrentDirectory";
            this.txtCurrentDirectory.Size = new System.Drawing.Size(402, 20);
            this.txtCurrentDirectory.TabIndex = 6;
            // 
            // lblCurrentDirectory
            // 
            this.lblCurrentDirectory.AutoSize = true;
            this.lblCurrentDirectory.Location = new System.Drawing.Point(3, 87);
            this.lblCurrentDirectory.Name = "lblCurrentDirectory";
            this.lblCurrentDirectory.Size = new System.Drawing.Size(84, 13);
            this.lblCurrentDirectory.TabIndex = 5;
            this.lblCurrentDirectory.Text = "Current directory";
            // 
            // txtArguments
            // 
            this.txtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArguments.Location = new System.Drawing.Point(3, 149);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(438, 20);
            this.txtArguments.TabIndex = 9;
            // 
            // lblArguments
            // 
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(3, 132);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(57, 13);
            this.lblArguments.TabIndex = 8;
            this.lblArguments.Text = "Arguments";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(366, 189);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(4, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(438, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(4, 2);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // ucServiceLikeConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.lblArguments);
            this.Controls.Add(this.btnCurrentDirectory);
            this.Controls.Add(this.txtCurrentDirectory);
            this.Controls.Add(this.lblCurrentDirectory);
            this.Controls.Add(this.btnExecutable);
            this.Controls.Add(this.txtExecutable);
            this.Controls.Add(this.lblExecutable);
            this.MinimumSize = new System.Drawing.Size(85, 215);
            this.Name = "ucServiceLikeConfig";
            this.Size = new System.Drawing.Size(445, 215);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExecutable;
        private System.Windows.Forms.TextBox txtExecutable;
        private System.Windows.Forms.Button btnExecutable;
        private System.Windows.Forms.Button btnCurrentDirectory;
        private System.Windows.Forms.TextBox txtCurrentDirectory;
        private System.Windows.Forms.Label lblCurrentDirectory;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.Button btnRemove;
        public System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
    }
}
