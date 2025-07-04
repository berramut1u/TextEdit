using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace TextEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;

            // Set up keyboard shortcuts in code:
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
        }

        private string currentFilePath = null;
        private bool isTextModified = false;
        private bool isInternalUpdate = false;



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                string fileContent = File.ReadAllText(filePath);

                textBox1.TextChanged -= textBox1_TextChanged;

                textBox1.Text = fileContent;
                currentFilePath = filePath;
                this.Text = $"TextEditor - {Path.GetFileName(filePath)}";

                isTextModified = false;

                textBox1.TextChanged += textBox1_TextChanged;
            }
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfirmExit())
            {
                textBox1.Clear();
                currentFilePath = null; 
                isTextModified = false; 
                this.Text = "TextEditor - New File";
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                File.WriteAllText(currentFilePath, textBox1.Text);
                isTextModified = false;
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.DefaultExt = "txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    File.WriteAllText(currentFilePath, textBox1.Text);
                    this.Text = $"TextEditor - {Path.GetFileName(currentFilePath)}";
                    isTextModified = false; 
                }
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfirmExit())
            {
                Application.Exit();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                saveToolStripMenuItem_Click(sender, e); 
                e.SuppressKeyPress = true; 
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private bool ConfirmExit()
        {
            if (isTextModified)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes before exiting?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, EventArgs.Empty);
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            return true; 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmExit())
            {
                e.Cancel = true; ,
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            textBox1.Paste();
        }
    }
}
