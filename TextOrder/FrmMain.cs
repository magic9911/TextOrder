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
            //masterHolderCtrl.Client = new ClientHolder("");
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
            controller.MasterControlHolder = masterHolderCtrl;
            controller.Form = this;

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

        public void setData(int time,int order) {

        }

        public void RefreshSlaves() {
            if (InvokeRequired) {
                Invoke(new Action(() => RefreshSlaves()));
                return;
            }
            flowLayoutPanel.SuspendLayout();

            IList<IClientHolder> slaves = controller.SlavesHolder;

            int slaveCtrlCount = flowLayoutPanel.Controls.Count;
            for (int i = 0,
                rCount = slaves.Count,
                cCount = slaveCtrlCount; i < rCount; i++) {

                if (i >= cCount) {
                    // ถ้าปุ่มไม่พอ เพิ่ม
                    flowLayoutPanel.Controls.Add(new ClientHolderCtrl(slaves[i]));
                } else {
                    // แก้ไขที่ปุ่มเดิม
                    // เพื่อให้ขณะ Refresh ไม่เกิด Form flicker
                    var slaveCtrl = flowLayoutPanel.Controls[i] as ClientHolderCtrl;
                    slaveCtrl.Client = slaves[i];
                }

            }

            // ลบ Room button ที่เหลือ (ถ้าไม่มีแล้ว)
            int remainCtrl = flowLayoutPanel.Controls.Count - slaves.Count;
            if (remainCtrl > 0) {
                for (int i = slaveCtrlCount - remainCtrl; i < slaveCtrlCount; i++) {
                    flowLayoutPanel.Controls.RemoveAt(i);
                }
            }
            flowLayoutPanel.ResumeLayout();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            btnStop.PerformClick();
        }
    }
}
