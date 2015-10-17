using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TextOrder.Holder;

namespace TextOrder {
    public partial class ClientHolderCtrl : UserControl {
        public ClientHolderCtrl() {
            InitializeComponent();

            Client = new ClientHolder("", "");
        }

        public IClientHolder Client;

        private void btnClose_Click(object sender, EventArgs e) {
            
        }

        private void btnBrowse_Click(object sender, EventArgs e) {
            var openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.Yes) {
                Client.FilePath = openDlg.FileName;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            Client.Name = txtName.Text;
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e) {

        }

        
    }
}
