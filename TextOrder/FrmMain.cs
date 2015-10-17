using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextOrder.Holder;

namespace TextOrder {
    public partial class FrmMain : Form {

        private Controller controller;
        private IList<IClientHolder> clients;

        public FrmMain() {
            InitializeComponent();

            clients = new List<IClientHolder>();

            // Initizlial Controller
            controller = new Controller(masterHolderCtrl.Client, clients);

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
            IClientHolder clientHolder = new ClientHolder("", "");
            clients.Add(clientHolder);
            flowLayoutPanel.Controls.Add(new ClientHolderCtrl(clientHolder));
        }

        private void masterHolderCtrl_ClosingControl(object sender, IClientHolder client) {
            clients.Remove(client);
        }
    }
}
