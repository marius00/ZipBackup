using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZipBackup.UI {
    public class TextBoxWithPaste : TextBox {
        public event EventHandler OnPaste;
        private const int WM_PASTE = 0x0302;

        protected override void WndProc(ref Message m) {
            if (m.Msg != WM_PASTE) {
                base.WndProc(ref m);
            } else {
                OnPaste?.Invoke(this, new PasteEventArg(Clipboard.GetText()));
            }
        }

        public class PasteEventArg : EventArgs {
            public PasteEventArg(string clipboard) {
                Clipboard = clipboard;
            }

            public string Clipboard { get; }
        }
    }
}
