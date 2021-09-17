using System;
using System.Windows.Forms;
using ZipBackup.Services;

namespace ZipBackup {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            ExceptionHandler.EnableLogUnhandledOnThread();

            tabControl.Dock = DockStyle.Fill;
        }
    }
}
