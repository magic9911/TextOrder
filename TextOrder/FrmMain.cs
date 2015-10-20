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
using YuriNET.CoreServer.Http;

namespace TextOrder {
    public partial class FrmMain : Form {

        private HttpController controller;

        public FrmMain() {
            InitializeComponent();
            
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
        
        private void startOperations() {
            // Initizlial Controller
            
            int port;
            if (! int.TryParse(txtPort.Text, out port)) {
                MessageBox.Show("Error port number !", Info.getProductName());
                return;
            }

            controller = new HttpController(port);
            controller.OnUpdateMaster += Controller_OnUpdateMaster;
            controller.OnUpdateSlaves += Controller_OnUpdateSlaves;
            controller.StartListen();
        }

        private void Controller_OnUpdateSlaves(object sender, UpdateSlavesEvenArgs holders) {
            //flowLayoutPanel.SuspendLayout();

            //int slaveCtrlCount = flowLayoutPanel.Controls.Count;
            //var Iterator = holders.DataHolders.ToList();
            //for (int i = 0,
            //    rCount = holders.DataHolders.Count,
            //    cCount = slaveCtrlCount; i < rCount; i++) {

            //    if (i >= cCount) {
            //        // ถ้าปุ่มไม่พอ เพิ่ม
            //        flowLayoutPanel.Controls.Add(new ClientHolderCtrl(Iterator[i]));
            //    } else {
            //        // แก้ไขที่ปุ่มเดิม
            //        // เพื่อให้ขณะ Refresh ไม่เกิด Form flicker
            //        var slaveCtrl = flowLayoutPanel.Controls[i] as ClientHolderCtrl;
            //        slaveCtrl.Client = Iterator[i].Value;
            //    }

            //}

            //// ลบ Room button ที่เหลือ (ถ้าไม่มีแล้ว)
            //int remainCtrl = flowLayoutPanel.Controls.Count - holders.DataHolders.Count;
            //if (remainCtrl > 0) {
            //    for (int i = slaveCtrlCount - remainCtrl; i < slaveCtrlCount; i++) {
            //        flowLayoutPanel.Controls.RemoveAt(i);
            //    }
            //}
            //flowLayoutPanel.ResumeLayout();
        }

        private void Controller_OnUpdateMaster(object sender, UpdateMasterEvenArgs holder) {
            masterHolderCtrl.Client = holder.DataHolder;
            masterHolderCtrl.Refresh();
        }

        private void stop() {
            //controller.StopMaster();
            controller.Stop();
        }

        public void setData(int time,int order) {

        }

        public void RefreshSlaves() {
            if (InvokeRequired) {
                Invoke(new Action(() => RefreshSlaves()));
                return;
            }
            
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            btnStop.PerformClick();
        }
    }
}
