using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CodeGenerator
{
    public class PrimaryForm : Form
    {
        public const string PREFERENCES_FILE = "CodeGeneratorPrefs.xml";

        private readonly CodeGeneratorController _controller;

        private readonly Translator _translator;

        private IContainer components = null;

        private Label label1;

        private TextBox txtDataFile;

        private Button btnBrowseData;

        private GroupBox groupBox1;

        private CheckedListBox cblTemplates;

        private Button btnBrowseTemplates;

        private Label label2;

        private TextBox txtOutputFolder;

        private Button btnBrowseOutput;

        private Button btnGenerateFiles;

        private ProgressBar pbGenStatus;

        private Label lblStatus;

        private OpenFileDialog ofdOpenFile;

        private FolderBrowserDialog fbdBrowseFolders;

        public PrimaryForm()
        {
            InitializeComponent();

            _translator = new Translator();
            _controller = new CodeGeneratorController(_translator);

            _translator.Error += _translator_Error;
            _translator.Notice += _translator_Notice;
            _translator.Progress += _translator_Progress;
        }

        private void btnBrowseData_Click(object sender, EventArgs e)
        {
            try
            {
                ofdOpenFile.Filter = "XML Data File(*.xml)|*.xml";
                ofdOpenFile.Title = "Select XML file to load..";
                ofdOpenFile.Multiselect = false;
                ofdOpenFile.InitialDirectory = txtDataFile.Text;
                if (ofdOpenFile.ShowDialog() == DialogResult.OK)
                {
                    _translator.DataFile = ofdOpenFile.FileName;
                    txtDataFile.Text = ofdOpenFile.FileName;
                }
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void btnBrowseTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                ofdOpenFile.Filter = "XSL Templates(*.xsl,*.xslt)|*.xsl;*.xslt";
                ofdOpenFile.Title = "Select templates to load..";
                ofdOpenFile.Multiselect = true;
                if (ofdOpenFile.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                _translator.Templates.Clear();
                cblTemplates.Items.Clear();
                string[] fileNames = ofdOpenFile.FileNames;
                foreach (string text in fileNames)
                {
                    _translator.Templates.Add(new Template(text, text, isSelected: true));
                }
                foreach (Template template in _translator.Templates)
                {
                    cblTemplates.Items.Add(template, isChecked: true);
                }
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            try
            {
                if (fbdBrowseFolders.ShowDialog() == DialogResult.OK)
                {
                    _translator.OutputFolder = fbdBrowseFolders.SelectedPath;
                    txtOutputFolder.Text = fbdBrowseFolders.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void cblTemplates_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (cblTemplates.SelectedItem != null)
                {
                    ((Template)cblTemplates.SelectedItem).IsSelected = e.NewValue == CheckState.Checked;
                }
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void btnGenerateFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDataFile.Text))
                {
                    NotifyError("You must provide a data file.", IsHandled: true);
                    return;
                }

                bool hasSelectedTemplate = false;
                foreach (Template template in _translator.Templates)
                {
                    if (template.IsSelected)
                    {
                        hasSelectedTemplate = true;
                        break;
                    }
                }

                if (!hasSelectedTemplate)
                {
                    NotifyError("You must have at least one template loaded and selected.", IsHandled: true);
                    return;
                }

                _translator.DataFile = txtDataFile.Text;
                _translator.OutputFolder = txtOutputFolder.Text;
                btnGenerateFiles.Enabled = false;

                _controller.GenerateFiles();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error Occurred";
                pbGenStatus.Value = 0;
                NotifyError(ex.Message);
            }
            finally
            {
                btnGenerateFiles.Enabled = true;
                btnGenerateFiles.Focus();
            }
        }

        private void _translator_Progress(object sender, TranslatorEventArgs e)
        {
            try
            {
                Invoke((ThreadStart)delegate
                {
                    pbGenStatus.Value = Convert.ToInt32(e.Progress);
                });
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void _translator_Notice(object sender, TranslatorEventArgs e)
        {
            try
            {
                Invoke((ThreadStart)delegate
                {
                    lblStatus.Text = e.Message;
                    Refresh();
                });
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void _translator_Error(object sender, TranslatorEventArgs e)
        {
            try
            {
                Invoke((ThreadStart)delegate
                {
                    NotifyError(e.Message, IsHandled: true);
                });
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void NotifyError(string message, bool IsHandled = false)
        {
            if (IsHandled)
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(message + "\rNOTE:  Program may not function properly.", "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void PrimaryForm_Shown(object sender, EventArgs e)
        {
            try
            {
                _controller.LoadOptions(PREFERENCES_FILE);

                txtDataFile.Text = _translator.DataFile;
                txtOutputFolder.Text = _translator.OutputFolder;

                foreach (Template template in _translator.Templates)
                {
                    cblTemplates.Items.Add(template, template.IsSelected);
                }
            }
            catch (Exception ex)
            {
                NotifyError(ex.Message);
            }
        }

        private void PrimaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Options options = new Options();
                options.OutputFolder = txtOutputFolder.Text;
                options.DataFile = txtDataFile.Text;
                foreach (Template template in _translator.Templates)
                {
                    if (template.IsSelected)
                    {
                        options.SelectedFiles.Add(template.Location);
                    }
                    else
                    {
                        options.UnSelectedFiles.Add(template.Location);
                    }
                }
                Utility.SerializeToXml(options, new FileInfo(PREFERENCES_FILE));
            }
            catch
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeGenerator.PrimaryForm));
            label1 = new System.Windows.Forms.Label();
            txtDataFile = new System.Windows.Forms.TextBox();
            btnBrowseData = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cblTemplates = new System.Windows.Forms.CheckedListBox();
            btnBrowseTemplates = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            txtOutputFolder = new System.Windows.Forms.TextBox();
            btnBrowseOutput = new System.Windows.Forms.Button();
            btnGenerateFiles = new System.Windows.Forms.Button();
            pbGenStatus = new System.Windows.Forms.ProgressBar();
            lblStatus = new System.Windows.Forms.Label();
            ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            fbdBrowseFolders = new System.Windows.Forms.FolderBrowserDialog();
            groupBox1.SuspendLayout();
            SuspendLayout();
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(34, 28);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(53, 14);
            label1.TabIndex = 0;
            label1.Text = "Data File";
            txtDataFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtDataFile.Location = new System.Drawing.Point(93, 25);
            txtDataFile.Name = "txtDataFile";
            txtDataFile.Size = new System.Drawing.Size(506, 22);
            txtDataFile.TabIndex = 0;
            btnBrowseData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowseData.Location = new System.Drawing.Point(605, 22);
            btnBrowseData.Name = "btnBrowseData";
            btnBrowseData.Size = new System.Drawing.Size(35, 25);
            btnBrowseData.TabIndex = 1;
            btnBrowseData.Text = "...";
            btnBrowseData.UseVisualStyleBackColor = true;
            btnBrowseData.Click += new System.EventHandler(btnBrowseData_Click);
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(cblTemplates);
            groupBox1.Controls.Add(btnBrowseTemplates);
            groupBox1.Location = new System.Drawing.Point(15, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(632, 194);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Translator Templates";
            cblTemplates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cblTemplates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            cblTemplates.FormattingEnabled = true;
            cblTemplates.Location = new System.Drawing.Point(6, 48);
            cblTemplates.Name = "cblTemplates";
            cblTemplates.Size = new System.Drawing.Size(620, 138);
            cblTemplates.TabIndex = 1;
            cblTemplates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(cblTemplates_ItemCheck);
            btnBrowseTemplates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowseTemplates.Location = new System.Drawing.Point(495, 16);
            btnBrowseTemplates.Name = "btnBrowseTemplates";
            btnBrowseTemplates.Size = new System.Drawing.Size(130, 25);
            btnBrowseTemplates.TabIndex = 0;
            btnBrowseTemplates.Text = "Load Templates..";
            btnBrowseTemplates.UseVisualStyleBackColor = true;
            btnBrowseTemplates.Click += new System.EventHandler(btnBrowseTemplates_Click);
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 282);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(82, 14);
            label2.TabIndex = 4;
            label2.Text = "Output Folder";
            txtOutputFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtOutputFolder.Location = new System.Drawing.Point(100, 279);
            txtOutputFolder.Name = "txtOutputFolder";
            txtOutputFolder.Size = new System.Drawing.Size(506, 22);
            txtOutputFolder.TabIndex = 2;
            btnBrowseOutput.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnBrowseOutput.Location = new System.Drawing.Point(612, 277);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new System.Drawing.Size(35, 25);
            btnBrowseOutput.TabIndex = 3;
            btnBrowseOutput.Text = "...";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Click += new System.EventHandler(btnBrowseOutput_Click);
            btnGenerateFiles.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btnGenerateFiles.Location = new System.Drawing.Point(261, 349);
            btnGenerateFiles.Name = "btnGenerateFiles";
            btnGenerateFiles.Size = new System.Drawing.Size(133, 43);
            btnGenerateFiles.TabIndex = 4;
            btnGenerateFiles.Text = "Generate Files";
            btnGenerateFiles.UseVisualStyleBackColor = true;
            btnGenerateFiles.Click += new System.EventHandler(btnGenerateFiles_Click);
            pbGenStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pbGenStatus.Location = new System.Drawing.Point(12, 318);
            pbGenStatus.Name = "pbGenStatus";
            pbGenStatus.Size = new System.Drawing.Size(635, 25);
            pbGenStatus.TabIndex = 8;
            lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblStatus.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblStatus.Location = new System.Drawing.Point(433, 363);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(207, 25);
            lblStatus.TabIndex = 9;
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new System.Drawing.Size(659, 404);
            base.Controls.Add(lblStatus);
            base.Controls.Add(pbGenStatus);
            base.Controls.Add(btnGenerateFiles);
            base.Controls.Add(btnBrowseOutput);
            base.Controls.Add(txtOutputFolder);
            base.Controls.Add(label2);
            base.Controls.Add(groupBox1);
            base.Controls.Add(btnBrowseData);
            base.Controls.Add(txtDataFile);
            base.Controls.Add(label1);
            Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "PrimaryForm";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Code Generation Utility";
            base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PrimaryForm_FormClosing);
            base.Shown += new System.EventHandler(PrimaryForm_Shown);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
