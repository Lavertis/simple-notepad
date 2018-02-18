using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Form1 : Form
    {
        String path = String.Empty;

        public Form1() => InitializeComponent();

        private string ReturnMessageFromFormat(string type)
        {
            switch (type)
            {
                case "ino":
                    return "Arduino";
                    break;
                case "cs":
                    return "C#";
                    break;
                case "cpp":
                    return "C++";
                    break;
                case "c":
                    return "C";
                    break;
                case "btwo":
                    return "Braintwo";
                    break;
                case "json":
                    return "Json";
                    break;
                case "xml":
                    return "Xml";
                    break;
                case "html":
                    return "HTML";
                    break;
                case "css":
                    return "CSS";
                    break;
                case "js":
                    return "JavaScript";
                    break;
                default:
                    return "Text";
                    break;

            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(path = openFileDialog1.FileName);
                string[] SplitExtension = openFileDialog1.FileName.Split('.');
                labelFormat.Text = ReturnMessageFromFormat(SplitExtension[1]);
               
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(path = saveFileDialog1.FileName, textBox1.Text);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, textBox1.Text);
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void exitPrompt()
        {
            DialogResult = MessageBox.Show("Do you want to save current file?",
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBox1.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    textBox1.Text = String.Empty;
                    path = String.Empty;;
                }
                else if (DialogResult == DialogResult.No)
                {
                    textBox1.Text = String.Empty;;
                    path = String.Empty;;
                }

            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.SelectAll();

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Cut();

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Copy();

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.Paste();

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => textBox1.SelectedText = String.Empty;


        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked == true)
            {
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                textBox1.WordWrap = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Font = textBox1.Font = new Font(fontDialog1.Font, fontDialog1.Font.Style);
                textBox1.ForeColor = fontDialog1.Color;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutForm = new Form2();
            aboutForm.ShowDialog();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                exitPrompt();

                if (DialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        e.SuppressKeyPress = true;
                        textBox1.SelectAll();
                        break;
                    case Keys.N:
                        e.SuppressKeyPress = true;
                        newToolStripMenuItem_Click(sender, e);
                        break;
                    case Keys.S:
                        e.SuppressKeyPress = true;
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.White;
            textBox1.BackColor = Color.Black;
            this.BackColor = Color.Gray;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.BackColor = Color.Gray;
            this.BackColor = Color.Gray;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.BackColor = Color.White;
            this.BackColor = Color.White;
        }
    }
}
