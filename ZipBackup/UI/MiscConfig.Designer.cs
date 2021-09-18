
namespace ZipBackup.UI {
    partial class MiscConfig {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.passwordInput2 = new System.Windows.Forms.MaskedTextBox();
            this.btnSavePassword = new System.Windows.Forms.Button();
            this.lbPatternPreview = new System.Windows.Forms.Label();
            this.lbFilePattern = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.passwordInput1 = new System.Windows.Forms.MaskedTextBox();
            this.tbFilePattern = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.passwordInput2);
            this.groupBox1.Controls.Add(this.btnSavePassword);
            this.groupBox1.Controls.Add(this.lbPatternPreview);
            this.groupBox1.Controls.Add(this.lbFilePattern);
            this.groupBox1.Controls.Add(this.lbPassword);
            this.groupBox1.Controls.Add(this.passwordInput1);
            this.groupBox1.Controls.Add(this.tbFilePattern);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 426);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Misc configurations";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // passwordInput2
            // 
            this.passwordInput2.Location = new System.Drawing.Point(111, 78);
            this.passwordInput2.Name = "passwordInput2";
            this.passwordInput2.Size = new System.Drawing.Size(223, 23);
            this.passwordInput2.TabIndex = 5;
            this.passwordInput2.Text = "--------";
            this.passwordInput2.UseSystemPasswordChar = true;
            // 
            // btnSavePassword
            // 
            this.btnSavePassword.Location = new System.Drawing.Point(337, 77);
            this.btnSavePassword.Name = "btnSavePassword";
            this.btnSavePassword.Size = new System.Drawing.Size(84, 23);
            this.btnSavePassword.TabIndex = 6;
            this.btnSavePassword.Text = "Save";
            this.btnSavePassword.UseVisualStyleBackColor = true;
            this.btnSavePassword.Click += new System.EventHandler(this.btnSavePassword_Click);
            // 
            // lbPatternPreview
            // 
            this.lbPatternPreview.AutoSize = true;
            this.lbPatternPreview.Location = new System.Drawing.Point(337, 23);
            this.lbPatternPreview.Name = "lbPatternPreview";
            this.lbPatternPreview.Size = new System.Drawing.Size(84, 15);
            this.lbPatternPreview.TabIndex = 2;
            this.lbPatternPreview.Text = "Preview: name";
            // 
            // lbFilePattern
            // 
            this.lbFilePattern.AutoSize = true;
            this.lbFilePattern.Location = new System.Drawing.Point(6, 23);
            this.lbFilePattern.Name = "lbFilePattern";
            this.lbFilePattern.Size = new System.Drawing.Size(99, 15);
            this.lbFilePattern.TabIndex = 0;
            this.lbFilePattern.Text = "Filename pattern:";
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(6, 52);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(77, 15);
            this.lbPassword.TabIndex = 3;
            this.lbPassword.Text = "Zip Password";
            // 
            // passwordInput1
            // 
            this.passwordInput1.Location = new System.Drawing.Point(111, 49);
            this.passwordInput1.Name = "passwordInput1";
            this.passwordInput1.Size = new System.Drawing.Size(223, 23);
            this.passwordInput1.TabIndex = 4;
            this.passwordInput1.Text = "********";
            this.passwordInput1.UseSystemPasswordChar = true;
            this.passwordInput1.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox1_MaskInputRejected);
            // 
            // tbFilePattern
            // 
            this.tbFilePattern.Location = new System.Drawing.Point(111, 20);
            this.tbFilePattern.Name = "tbFilePattern";
            this.tbFilePattern.Size = new System.Drawing.Size(223, 23);
            this.tbFilePattern.TabIndex = 1;
            this.tbFilePattern.TextChanged += new System.EventHandler(this.tbFilePattern_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // MiscConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MiscConfig";
            this.Text = "MiscConfig";
            this.Load += new System.EventHandler(this.MiscConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbFilePattern;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.MaskedTextBox passwordInput1;
        private System.Windows.Forms.TextBox tbFilePattern;
        private System.Windows.Forms.Label lbPatternPreview;
        private System.Windows.Forms.Button btnSavePassword;
        private System.Windows.Forms.MaskedTextBox passwordInput2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}