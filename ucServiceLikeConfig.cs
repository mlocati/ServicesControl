using System;
using System.IO;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    public partial class ucServiceLikeConfig : UserControl
    {
        public class ConfigException : Exception
        {
            public readonly Control Control;
            public ConfigException(string message, Control control)
                : base(message)
            {
                this.Control = control;
            }
        }
        public event EventHandler AskRemove;
        public ucServiceLikeConfig() : this(null)
        {

        }
        public ucServiceLikeConfig(ServiceLikeServiceConfig config)
        {
            InitializeComponent();
            this.txtName.Text = config == null || config.ServiceName == null ? "" : config.ServiceName;
            this.txtExecutable.Text = config == null || config.Executable == null ? "" : config.Executable;
            this.txtCurrentDirectory.Text = config == null || config.CurrentDirectory == null ? "" : config.CurrentDirectory;
            this.txtArguments.Text = config == null || config.Arguments == null ? "" : config.Arguments;
        }

        /// <exception cref="MLocati.ServicesControl.ucServiceLikeConfig.ConfigException">if some field is misconfigured</exception>
        public ServiceLikeServiceConfig GetConfig()
        {
            var name = this.txtName.Text.Trim();
            if (name.Length == 0)
            {
                throw new ConfigException("Please specify the name", this.txtName);
            }
            var executable = this.txtExecutable.Text.Trim();
            if (executable.Length == 0)
            {
                throw new ConfigException("Please specify the program", this.txtExecutable);
            }
            if (!File.Exists(executable))
            {
                throw new ConfigException($"File not found: {executable}", this.txtExecutable);
            }
            var currentDirectory = this.txtCurrentDirectory.Text.Trim();
            if (currentDirectory.Length > 0 && !Directory.Exists(currentDirectory))
            {
                throw new ConfigException($"Directory not found: {currentDirectory}", this.txtCurrentDirectory);
            }
            return new ServiceLikeServiceConfig(name, executable, currentDirectory, this.txtArguments.Text.Trim());
        }

        private void txtExecutable_Leave(object sender, EventArgs e)
        {
            this.AutosetCurrentDirectory();
        }

        private void btnExecutable_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select executable";
                dialog.Filter = "Programs (*.exe)|*.exe|All files|*.*";
                dialog.FileName = this.txtExecutable.Text;
                try
                {
                    var dir = this.txtExecutable.Text;
                    for (; ; )
                    {
                        dir = Path.GetDirectoryName(dir);
                        if (string.IsNullOrEmpty(dir))
                        {
                            break;
                        }
                        if (Directory.Exists(dir))
                        {
                            dialog.InitialDirectory = dir;
                            break;
                        }
                    }
                }
                catch { }
                dialog.FileName = this.txtExecutable.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.txtExecutable.Text = dialog.FileName;
                    this.AutosetCurrentDirectory();
                }
            }
        }

        private void btnCurrentDirectory_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the current directory";
                dialog.SelectedPath = this.txtCurrentDirectory.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.txtCurrentDirectory.Text = dialog.SelectedPath;
                }
            }
        }

        private void AutosetCurrentDirectory()
        {
            var filePath = Path.GetFullPath(this.txtExecutable.Text);
            if (File.Exists(filePath))
            {
                this.txtCurrentDirectory.Text = Path.GetDirectoryName(filePath);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.AskRemove != null)
            {
                this.AskRemove(this, new EventArgs());
            }
        }
    }
}
