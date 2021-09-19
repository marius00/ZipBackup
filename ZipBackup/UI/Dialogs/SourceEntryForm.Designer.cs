
namespace ZipBackup.UI {
    partial class SourceEntryForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceEntryForm));
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbExclusionFilter = new System.Windows.Forms.TextBox();
            this.tbInclusionFilter = new System.Windows.Forms.TextBox();
            this.tbPath = new ZipBackup.UI.TextBoxWithPaste();
            this.lbInclusionFilter = new System.Windows.Forms.Label();
            this.lbExclusionFilter = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbCompression = new System.Windows.Forms.Label();
            this.cbCompression = new System.Windows.Forms.ComboBox();
            this.cbIncludeSubfodlers = new System.Windows.Forms.CheckBox();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(107, 22);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(573, 23);
            this.tbName.TabIndex = 1;
            // 
            // tbExclusionFilter
            // 
            this.tbExclusionFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExclusionFilter.Location = new System.Drawing.Point(107, 109);
            this.tbExclusionFilter.Name = "tbExclusionFilter";
            this.tbExclusionFilter.Size = new System.Drawing.Size(573, 23);
            this.tbExclusionFilter.TabIndex = 8;
            this.tbExclusionFilter.TextChanged += new System.EventHandler(this.tbExclusionFilter_TextChanged);
            // 
            // tbInclusionFilter
            // 
            this.tbInclusionFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInclusionFilter.Location = new System.Drawing.Point(107, 80);
            this.tbInclusionFilter.Name = "tbInclusionFilter";
            this.tbInclusionFilter.Size = new System.Drawing.Size(573, 23);
            this.tbInclusionFilter.TabIndex = 6;
            this.tbInclusionFilter.TextChanged += new System.EventHandler(this.tbInclusionFilter_TextChanged);
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(107, 51);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(439, 23);
            this.tbPath.TabIndex = 3;
            // 
            // lbInclusionFilter
            // 
            this.lbInclusionFilter.AutoSize = true;
            this.lbInclusionFilter.Location = new System.Drawing.Point(14, 83);
            this.lbInclusionFilter.Name = "lbInclusionFilter";
            this.lbInclusionFilter.Size = new System.Drawing.Size(87, 15);
            this.lbInclusionFilter.TabIndex = 5;
            this.lbInclusionFilter.Text = "Inclusion Filter:";
            // 
            // lbExclusionFilter
            // 
            this.lbExclusionFilter.AutoSize = true;
            this.lbExclusionFilter.Location = new System.Drawing.Point(14, 112);
            this.lbExclusionFilter.Name = "lbExclusionFilter";
            this.lbExclusionFilter.Size = new System.Drawing.Size(89, 15);
            this.lbExclusionFilter.TabIndex = 7;
            this.lbExclusionFilter.Text = "Exclusion Filter:";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(14, 25);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(39, 15);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name";
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Location = new System.Drawing.Point(14, 54);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(31, 15);
            this.lbPath.TabIndex = 2;
            this.lbPath.Text = "Path";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lbCompression);
            this.groupBox1.Controls.Add(this.cbCompression);
            this.groupBox1.Controls.Add(this.cbIncludeSubfodlers);
            this.groupBox1.Controls.Add(this.btnBrowsePath);
            this.groupBox1.Controls.Add(this.lbExclusionFilter);
            this.groupBox1.Controls.Add(this.lbPath);
            this.groupBox1.Controls.Add(this.tbExclusionFilter);
            this.groupBox1.Controls.Add(this.lbInclusionFilter);
            this.groupBox1.Controls.Add(this.tbName);
            this.groupBox1.Controls.Add(this.tbInclusionFilter);
            this.groupBox1.Controls.Add(this.lbName);
            this.groupBox1.Controls.Add(this.tbPath);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 197);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Backup Source";
            // 
            // lbCompression
            // 
            this.lbCompression.AutoSize = true;
            this.lbCompression.Location = new System.Drawing.Point(14, 141);
            this.lbCompression.Name = "lbCompression";
            this.lbCompression.Size = new System.Drawing.Size(80, 15);
            this.lbCompression.TabIndex = 9;
            this.lbCompression.Text = "Compression:";
            // 
            // cbCompression
            // 
            this.cbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompression.FormattingEnabled = true;
            this.cbCompression.Items.AddRange(new object[] {
            "Optimal    ",
            "Fastest ",
            "NoCompression"});
            this.cbCompression.Location = new System.Drawing.Point(107, 138);
            this.cbCompression.Name = "cbCompression";
            this.cbCompression.Size = new System.Drawing.Size(158, 23);
            this.cbCompression.TabIndex = 10;
            // 
            // cbIncludeSubfodlers
            // 
            this.cbIncludeSubfodlers.AutoSize = true;
            this.cbIncludeSubfodlers.Location = new System.Drawing.Point(14, 167);
            this.cbIncludeSubfodlers.Name = "cbIncludeSubfodlers";
            this.cbIncludeSubfodlers.Size = new System.Drawing.Size(123, 19);
            this.cbIncludeSubfodlers.TabIndex = 11;
            this.cbIncludeSubfodlers.Text = "Include subfolders";
            this.cbIncludeSubfodlers.UseVisualStyleBackColor = true;
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowsePath.Location = new System.Drawing.Point(552, 51);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(128, 23);
            this.btnBrowsePath.TabIndex = 4;
            this.btnBrowsePath.Text = "Browse..";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(6, 217);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 40);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(609, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 40);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // SourceEntryForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(721, 263);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SourceEntryForm";
            this.Text = "Backup Source..";
            this.Load += new System.EventHandler(this.SourceEntryForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbExclusionFilter;
        private System.Windows.Forms.TextBox tbInclusionFilter;
        private ZipBackup.UI.TextBoxWithPaste tbPath;
        private System.Windows.Forms.Label lbInclusionFilter;
        private System.Windows.Forms.Label lbExclusionFilter;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowsePath;
        private System.Windows.Forms.Label lbCompression;
        private System.Windows.Forms.ComboBox cbCompression;
        private System.Windows.Forms.CheckBox cbIncludeSubfodlers;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}