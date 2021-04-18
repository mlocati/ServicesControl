using System;
using System.Drawing;
using System.Windows.Forms;

namespace MLocati.ServicesControl
{
    public partial class frmProgramOutputViewer : Form
    {
        private readonly ProgramOutput Output;

        public frmProgramOutputViewer(ProgramOutput output)
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.Output = output;
            this.rtbOutput.SuspendLayout();
            foreach (var chunk in this.Output.Chunks)
            {
                this.ViewChunk(chunk, false);
            }
            this.rtbOutput.ResumeLayout();
            this.Output.Cleared += this.Output_Cleared;
            this.Output.ChunkAdded += Output_ChunkAdded;
        }

        private void Output_ChunkAdded(object sender, ProgramOutput.ChunkAddedEventArgs e)
        {
            if (this.rtbOutput.InvokeRequired)
            {
                this.rtbOutput.Invoke((MethodInvoker)delegate
                {
                    this.ViewChunk(e.Chunk, true);
                });
            }
            else
            {
                this.ViewChunk(e.Chunk, true);
            }
        }

        private void Output_Cleared(object sender, EventArgs e)
        {
            if (this.rtbOutput.InvokeRequired)
            {
                this.rtbOutput.Invoke((MethodInvoker)delegate
               {
                   this.rtbOutput.Clear();
               });
            }
            else
            {
                this.rtbOutput.Clear();
            }
        }

        private void ViewChunk(ProgramOutput.Chunk chunk, bool suspendLayout)
        {
            if (suspendLayout)
            {
                this.rtbOutput.SuspendLayout();
            }
            var oldStart = this.rtbOutput.SelectionStart;
            var oldLength = this.rtbOutput.SelectionLength;
            this.rtbOutput.SelectionStart = this.rtbOutput.TextLength;
            this.rtbOutput.SelectionLength = 0;
            this.rtbOutput.SelectionColor = chunk.Type == ProgramOutput.Type.StdErr ? Color.Red : this.rtbOutput.ForeColor;
            this.rtbOutput.AppendText(chunk.Text);
            this.rtbOutput.SelectionColor = this.rtbOutput.ForeColor;
            this.rtbOutput.AppendText(Environment.NewLine);
            this.rtbOutput.SelectionStart = oldStart;
            this.rtbOutput.SelectionLength = oldLength;
            if (suspendLayout)
            {
                this.rtbOutput.ResumeLayout();
            }
        }

        private void frmProgramOutputViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Output.Cleared -= this.Output_Cleared;
            this.Output.ChunkAdded -= this.Output_ChunkAdded;
        }
    }
}
