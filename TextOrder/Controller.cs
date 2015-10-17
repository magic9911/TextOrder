using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextOrder.Holder;

namespace TextOrder {
    public class Controller {

        private Thread masterTask;
        private IClientHolder masterHolder;
        private IList<IClientHolder> slavesHolder;
        private bool started;

        public IClientHolder MasterHolder { get; set; }
        public IList<IClientHolder> SlavesHolder;
        
        public Controller(IClientHolder master, IList<IClientHolder> slaves) {
            // Initialization

            masterHolder = master;
            slavesHolder = slaves;
        }

        /// <summary>
        /// Thread
        /// </summary>
        private void loopMaster() {
            while (started) {
                // Loop Check master file
                if (!string.IsNullOrEmpty(masterHolder.Contents.RawData)) {
                    // น่าจะพบคำสั่ง  เรียกส่งคำสั่งให้ลูก
                    SendMessageToSlaves(masterHolder.Contents);
                }

                // Sleep ไม่ให้ CPU สูง
                Thread.Sleep(1);
            }
        }


        public void SendMessageToSlaves(ClientData contents) {
            foreach (var slave in slavesHolder) {
                slave.Contents = contents;
                var task = new Task(() => {
                    SendMessageToSlave(slave);
                });
                task.Start();
            }
        }

        /// <summary>
        /// Thread write file
        /// </summary>
        /// <param name="client"></param>
        public void SendMessageToSlave(IClientHolder client) {
            
        }

        public void StartMaster() {
            // สร้าง Thread
            masterTask = new Thread(() => {
                loopMaster();
            });

            masterTask.Start();
        }

        public void StopMaster() {
            // ให้ออก Loop เอง
            started = false;
        }

    }
}
