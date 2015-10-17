using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextOrder {
    public partial class FrmMain : Form {
        public FrmMain() {
            InitializeComponent();
        }

        

        private void btnStart_Click(object sender, EventArgs e) {


            btnStop.Enabled = btnStart.Enabled;
            btnStart.Enabled = !btnStop.Enabled;
        }

        private void btnStop_Click(object sender, EventArgs e) {

            btnStop.Enabled = btnStart.Enabled;
            btnStart.Enabled = !btnStop.Enabled;
        }

        private void btnAddSlave_Click(object sender, EventArgs e) {
            flowLayoutPanel.Controls.Add(new ClientHolderCtrl());
        }
    }
}
