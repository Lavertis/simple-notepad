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
    public partial class MainForm : Form
    {

        public MainForm() => InitializeComponent();

        /// <summary>
        /// Saves a file
        /// </summary>
        private async Task saveFileAsync(bool isSaveAs = false)
        {
            // path not being null means FileDialog was already opened and a path select, so a file is being modified, not created
            if (isSaveAs || String.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                saveFileDialog.ShowDialog();
            }
            else
            {
                await Task.Run(() => File.WriteAllText(openFileDialog.FileName, mainTextBox.Text));
                openFileDialog.FileName = "";
            }

            emptyMainTextBox();
        }

        /// <summary>
        /// Open a message box with the given message and Yes, No and Cancel buttons
        /// </summary>
        /// <param name="msg">Message to be shown to the user</param>
        private DialogResult yesNoCancelMessageBox(string msg)
        {
            return MessageBox.Show(
                msg,
                "Notepad",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// If there's any text in the mainTextBox, asks user wheter or not he wants to save the current file, then empty the mainTextBox and path
        /// </summary>
        private async Task newFile(object sender)
        {
            if (String.IsNullOrWhiteSpace(mainTextBox.Text) == false)
            {
                DialogResult result = yesNoCancelMessageBox("Would you like to save the current file?");
                if (result == DialogResult.Yes)
                {
                    await saveFileAsync();
                }

                emptyMainTextBox();

            }
        }

        /// <summary>
        /// Select all text in mainTextBox
        /// </summary>
        private void selectAllMainTextBox()
        {
            mainTextBox.SelectAll();
        }

        /// <summary>
        /// Empty mainTextBox's text
        /// </summary>
        private void emptyMainTextBox()
        {
            mainTextBox.Text = "";
        }

        /// <summary>
        /// Opens font changing dialog box and change mainTextBox's text accordingly
        /// </summary>
        private void changeFontByDialogBox()
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                mainTextBox.Font = mainTextBox.Font = new Font(fontDialog.Font, fontDialog.Font.Style);
                mainTextBox.ForeColor = fontDialog.Color;
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked)
            {
                mainTextBox.WordWrap = false;
                mainTextBox.ScrollBars = ScrollBars.Both;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                mainTextBox.WordWrap = true;
                mainTextBox.ScrollBars = ScrollBars.Vertical;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }

        /// <summary>
        /// Handles clicking the open button located in the main tool strip
        /// </summary>
        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mainTextBox.Text = await Task.Run(() => File.ReadAllText(openFileDialog.FileName));
                string[] SplitExtension = openFileDialog.FileName.Split('.');
                labelFormat.Text = NotepadProcessor.ReturnMessageFromFormat(SplitExtension[1]);
            }
        }

        /// <summary>
        /// Shows "About" form as dialog box
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        /// <summary>
        /// Handles clicking the exit button located in the main tool strip
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        /// <summary>
        /// Handles clicking the new button located in the main tool strip
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile(sender);
        }

        /// <summary>
        /// Handles clicking the select all button located in the main tool strip
        /// </summary>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => selectAllMainTextBox();

        /// <summary>
        /// Handles clicking the cut button located in the main tool strip
        /// </summary>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => mainTextBox.Cut();

        /// <summary>
        /// Handles clicking the copy button located in the main tool strip
        /// </summary>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => mainTextBox.Copy();

        /// <summary>
        /// Handles clicking the paste button located in the main tool strip
        /// </summary>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => mainTextBox.Paste();

        /// <summary>
        /// Handles clicking the delete button located in the main tool strip
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => emptyMainTextBox();

        /// <summary>
        /// Handles clicking on Font button located in the main tool strip
        /// </summary>
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeFontByDialogBox();
        }

        /// <summary>
        /// Handles closing the form asking wheter or not the user wants to save current text (if there's any)
        /// </summary>
        private async void mainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mainTextBox.Text))
            {
                yesNoCancelMessageBox("Would you like to save the file before leaving?");

                if (DialogResult == DialogResult.Yes)
                {
                    await saveFileAsync();
                }
                else if (DialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles CTRL + Key functions. Key = A, for selecting all text. Key = N, for new file. Key = S, for saving all text as file. 
        /// </summary>
        private async void mainTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.O:
                        e.SuppressKeyPress = true;
                        openToolStripMenuItem_Click(this, new EventArgs());
                        break;
                    case Keys.A:
                        e.SuppressKeyPress = true;
                        selectAllMainTextBox();
                        break;
                    case Keys.N:
                        e.SuppressKeyPress = true;
                        newFile(sender);
                        break;
                    case Keys.S:
                        e.SuppressKeyPress = true;
                        await saveFileAsync();
                        break;
                }
            }
        }

        /// <summary>
        /// Handles clicking on OK button located inside the SaveFileDialog
        /// </summary>
        private async void saveFileDialog_Click(object sender, EventArgs e)
        {
            await Task.Run(() => File.WriteAllText(saveFileDialog.FileName, mainTextBox.Text));
        }

        /// <summary>
        /// Handles "save" button click
        /// </summary> 
        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await saveFileAsync();
        }

        /// <summary>
        /// Handles "save as" button click
        /// </summary> 
        private async void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await saveFileAsync(true);
        }

        /// <summary>
        /// Responsable for changing the object's colors based on the given colors according to the selected theme
        /// </summary>
        /// <param name="mainTextBoxColor">Editor's text box fore color</param>
        /// <param name="mainTextBoxBkColor">Editor's text box back color</param>
        /// <param name="bkColor">Main window's background color</param>
        private void ThemeChangeObjectsColor(Color mainTextBoxColor, Color mainTextBoxBkColor, Color? bkColor = null)
        {
            mainTextBox.ForeColor = mainTextBoxColor;
            mainTextBox.BackColor = mainTextBoxBkColor;
            this.BackColor = bkColor is null ? mainTextBoxBkColor : bkColor.Value;
        }

        /// <summary>
        /// gathered every theme change in one method
        /// </summary>
        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                // black theme
                case "Black":
                    ThemeChangeObjectsColor(Color.White, Color.Black, Color.Gray);
                    break;
                // gray theme
                case "Gray":
                    ThemeChangeObjectsColor(Color.Black, Color.Gray);
                    break;
                // default theme
                default:
                    ThemeChangeObjectsColor(Color.Black, Color.White);
                    break;
            }
        }

    }
}
