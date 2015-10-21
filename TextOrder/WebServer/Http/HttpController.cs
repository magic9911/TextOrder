using GoldStar.Lib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextOrder;
using TextOrder.Holder;

namespace YuriNET.CoreServer.Http {

    internal class HttpController : HttpServer {
        private readonly Logger Logger = Logger.getInstance();

        // Business
        private DictData masterData;
        //private DictData slavesHolder;

        // System
        private Thread runnerThread;
        private bool maintenace = false;
        private int timeout = 30;

        // UI
        //public ClientHolderCtrl MasterControlHolder;

        // Event
        public delegate void OnUpdateDelegate(object sender, UpdateMasterEvenArgs holder);
        public event OnUpdateDelegate OnUpdateMaster;
        public delegate void OnUpdateSlavesDelegate(object sender, UpdateSlavesEvenArgs holders);
        public event OnUpdateSlavesDelegate OnUpdateSlaves;

        /// <summary>
        /// Constructor HTTP Controller
        /// </summary>
        /// <param name="port"></param>
        public HttpController(int port)
            : base(port) {
            // Initialize
            masterData = new DictData();
            //slavesHolder = new DictData(); เดี๋ยวทำ เก็บ client

        }

        public void StartListen() {
            bind();
        }

        public override void Stop() {
            base.Stop();
            //runnerThread.Abort();
        }

        /// <summary>
        /// รับ Request GET
        /// </summary>
        /// <param name="p"></param>
        public override void handleGETRequest(HttpProcessor p) {
            // Header HTTP
            string[] segments = p.URI.Segments;
            IDictionary<string, string> parameters = p.http_query;


            // Response text
            StringBuilder response = new StringBuilder();

            if (parameters["mode"] == "close") {
                string[] CloseID = parameters["positions"].Split('|').Select((ary) => ary.Split(';')[0]).ToArray();

                foreach (var id in CloseID) {
                    if (id != "") {
                        masterData.Remove(id);
                    }
                }
                response.Append("[Close-OK]");

            } else if (parameters["mode"] == "save") {

                string[] positions = parameters["positions"].Split('|');

                foreach (var position in positions) {
                    if (position != "") {
                        string id = position.Split(';')[0];

                        if (masterData.ContainsKey(id)) {
                            masterData[id]  = position;
                        } else {
                            masterData.Add(id,position);
                        }
                    }
                }
                response.Append("[Save-OK]");
            } else if (parameters["mode"] == "client") {
                if (masterData.Count > 0) {
                    string Symbol = parameters["symbol"];
                    // เหลือ ค้นตรงนี้ ถ้า symbol ส่ง แค่่อันเดียว
                    foreach (var data in masterData) {
                            response.Append(data.Value.ToString());
                    }
                }

            } else if (parameters["mode"] == "checkOrder") {
                if (masterData.Count > 0) {
                    string OrderID = parameters["id"];
                        if (OrderID != "") {
                           response.Append(masterData[OrderID].ToString());
                        }
                }
            }

            //ยิง Event
            //if (null != OnUpdateSlaves) {
            //    OnUpdateSlaves(this, new UpdateSlavesEvenArgs() {
            //        Master = true,
            //        DataHolders = slavesHolder
            //    });
            //}

            // data//
            ///  /; master = true; accountid = 8595808; time = 1445030830; positions = []; balance = 63.46; equity = 63.46; end = 0 //
            /// 
            //
            // positions =      int           int          double       string      double   strint      double          double        double      int
            // positions = [OrderOpenTime;OrderTicket;OrderOpenPrice;OrderSymbol;OrderLots;OrderType;OrderStopLoss;OrderTakeProfit;OrderProfit;AccountNumber];
            //
            //   ถ้า master = true ให้เก็บค่า ที่ส่งมา , แสดงเวลา master บน form "time = 1445030830"
            //   ถ้า master = false ให้แสดง ออเดอร์ ของ มาสเตอร์ คือ positions



            // Condition
            //if (maintenace) {
            //    Logger.debug("Server Maintenace !");
            //    p.write503();
            //    p.outputStream.WriteLine("503 Server Temporarily Unavailable");
            //    return;
            //}

            // Send back to client
            p.writeSuccess();
            p.outputStream.WriteLine(response.ToString());
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData) {
            Logger.debug("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();

            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
        }

        private void runner() {
            Logger.info("Controller started...");

            //long lastHeartbeat = 0;
            //bool connected = false;

            //while (myServer.isServerStarted()) {
            // Shutdown on maintenace mode
            //if (maintenace && clients.IsEmpty) {
            //    Logger.info("Tunnel empty, doing maintenace quit.");
            //    myServer.stopServer();
            //    return;
            //}

            // TODO: Send heartbeat to master server.

            // Removing timeout clients
            //var timeouts = clients
            //   .Where(kvp => {
            //       return DateTimeUtil.checkTimeout(kvp.Value.getTimestamp(), (long) timeout * 1000);
            //   }).ToList();
            //if (timeouts.Count > 0) {
            //    foreach (var kvp in timeouts) {
            //        Logger.info("Disconnect client {0} timed out.", kvp.Value.ToString());
            //        Client client;
            //        clients.TryRemove(kvp.Key, out client);
            //        pool.TryAdd(kvp.Key);
            //        client.Dispose();
            //    }
            //}

            // TODO: Locks Unlocking clients

            // Set peek
            //var clientscount = getClientsCount();
            //if (clientscount > peekClients) {
            //    peekClients = clientscount;
            //}

            //Logger.info("Clients : {0} / {1} players online.", clients.Count, maxClients);
            //    Thread.Sleep(5000);
            //}
        }
    }

    class UpdateMasterEvenArgs : EventArgs {
        public bool Master;
        public ClientHolder DataHolder;
    }

    class UpdateSlavesEvenArgs : EventArgs {
        public bool Master;
        public DictData DataHolders;
    }
}