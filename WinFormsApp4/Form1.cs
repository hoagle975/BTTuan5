using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSystemFonts();
            LoadFontSizes();
            ResetToDefault();
        }

        private void LoadSystemFonts()
        {
            cmbFonts.Items.Clear();
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            foreach (FontFamily font in installedFonts.Families)
            {
                cmbFonts.Items.Add(font.Name);
            }
        }

        private void LoadFontSizes()
        {
            cmbSizes.Items.Clear();
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in sizes)
            {
                cmbSizes.Items.Add(size);
            }
        }

        private void ResetToDefault()
        {
            richText.Clear();
            currentFilePath = string.Empty;

            cmbFonts.SelectedItem = "Tahoma";
            cmbSizes.SelectedItem = 14;

            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);

            btnBold.Checked = false;
            btnItalic.Checked = false;
            btnUnderline.Checked = false;

            UpdateWordCount();
        }

        private void btnTaoMoi_Click(object sender, EventArgs e)
        {
            ResetToDefault();
        }

        private void btnMoTapTin_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                string extension = Path.GetExtension(currentFilePath).ToLower();

                if (extension == ".rtf")
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                UpdateWordCount();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                string extension = Path.GetExtension(currentFilePath).ToLower();
                if (extension == ".rtf")
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDinhDang_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (richText.SelectionFont != null)
                fontDlg.Font = richText.SelectionFont;
            fontDlg.Color = richText.SelectionColor;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.SelectionColor = fontDlg.Color;
                richText.SelectionFont = fontDlg.Font;

                if (fontDlg.Font != null)
                {
                    cmbFonts.SelectedItem = fontDlg.Font.Name;
                    cmbSizes.SelectedItem = (int)fontDlg.Font.Size;
                }
            }
        }

        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFonts.SelectedItem != null && richText.SelectionFont != null)
            {
                string newFontName = cmbFonts.SelectedItem.ToString();
                float currentSize = richText.SelectionFont.Size;
                FontStyle currentStyle = richText.SelectionFont.Style;

                richText.SelectionFont = new Font(newFontName, currentSize, currentStyle);
            }
        }

        private void cmbSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSizes.SelectedItem != null && richText.SelectionFont != null)
            {
                float newSize = Convert.ToSingle(cmbSizes.SelectedItem);
                string currentFontName = richText.SelectionFont.Name;
                FontStyle currentStyle = richText.SelectionFont.Style;

                richText.SelectionFont = new Font(currentFontName, newSize, currentStyle);
            }
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
        }

        private void ToggleFontStyle(FontStyle style)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ style;

                richText.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            string text = richText.Text.Trim();
            int wordCount = 0;

            if (!string.IsNullOrEmpty(text))
            {
                string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                wordCount = words.Length;
            }

            lblWordCount.Text = $"Tổng số từ: {wordCount}";
        }
    }
}