using GoldStar.Lib.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            
        }
        
        private void btnStart_Click(object sender, EventArgs e) {
            
            btnStop.Enabled = btnStart.Enabled;
            btnStart.Enabled = !btnStop.Enabled;
            new Thread(() => startOperations()).Start();

            
        }

        private void btnStop_Click(object sender, EventArgs e) {

            btnStop.Enabled = btnStart.Enabled;
            btnStart.Enabled = !btnStop.Enabled;
            stop();
        }

        private void btnAddSlave_Click(object sender, EventArgs e) {
            IClientHolder clientHolder = new ClientHolder("");
            clients.Add(clientHolder);
            flowLayoutPanel.Controls.Add(new ClientHolderCtrl(clientHolder));
        }

        private void masterHolderCtrl_ClosingControl(object sender, IClientHolder client) {
            clients.Remove(client);
        }

        private void startOperations() {
            // Initizlial Controller
            controller = new Controller(masterHolderCtrl.Client, clients);

            int port;
            if (! int.TryParse(txtPort.Text, out port)) {
                MessageBox.Show("Error port number !", Info.getProductName());
                return;
            }
            controller.ServerPort = port;
            controller.StartMaster();
        }

        private void stop() {
            //controller.StopMaster();
            controller.Dispose();
        }
    }
}
