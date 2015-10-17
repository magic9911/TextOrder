using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextOrder.Holder;
using YuriNET.CoreServer.Http;

namespace TextOrder {
    public class Controller : IDisposable {

        private Thread masterTask;
        private IClientHolder masterHolder;
        private IList<IClientHolder> slavesHolder;
        private HttpController http;
        private bool started;
        

        public IClientHolder MasterHolder { get; set; }
        public IList<IClientHolder> SlavesHolder;
        public int ServerPort;

        public Controller(IClientHolder master, IList<IClientHolder> slaves) {
            // Initialization

            masterHolder = master;
            slavesHolder = slaves;

            // Default values
            ServerPort = 82;
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

            http = new HttpController(ServerPort);
            http.listen();
        }

        public void StopMaster() {
            // ให้ออก Loop เอง
            started = false;
            http.stop();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                    if (null != http) {
                        http.stop();
                        http = null;
                    }
                    slavesHolder.Clear();
                    slavesHolder = null;
                    masterHolder = null;

                    if (null != masterTask) {
                        if (masterTask.ThreadState != ThreadState.Stopped) {
                            try {
                                masterTask.Abort();
                            } catch { }
                            
                            masterTask = null;
                        }
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Controller() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
