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


        private Controller controller;
        private Thread runnerThread;

        private bool maintenace = false;
        private int timeout = 30;
        //private int maxClients = 90;
        //private int peekClients = 0;
        //private string password = null;

        public HttpController(Controller controller, int port)
            : base(port) {
            this.controller = controller;
        }

        public override void stop() {
            base.stop();
            //runnerThread.Abort();
        }

        //public Client getClient(short clientId) {
        //    Client client;
        //    Logger.debug("getClient({0})", clientId);
        //    clients.TryGetValue(clientId, out client);
        //    Logger.debug("client : {0}", client);
        //    return client;
        //}

        //public int getClientsCount() {
        //    return clients.Where(kvp => kvp.Value != null).ToList().Count;
        //}

        //public int getPeekClients() {
        //    return peekClients;
        //}

        public override void handleGETRequest(HttpProcessor p) {
            // Header

            string[] segments = p.URI.Segments;
            IDictionary<string, string> parameters = p.http_query;
            IList<ClientData> masterDatas = controller.MasterHolder.Contents;

            // Response text
            StringBuilder response = new StringBuilder();

            if (parameters["master"] == "true") {
                string[] positions = parameters["positions"].Split('|');

                
                controller.MasterHolder.Account = parameters["accountid"];
                controller.RefreshUI();

                foreach (var item in positions) {
                    ClientData find = masterDatas.Where((c) => c.RawData == item).FirstOrDefault();
                    if (null != find) {
                        find.MapData(item);
                    } else {
                        masterDatas.Add(new ClientData(item));
                    }
                    
                }
                response.Append("[OK]");
            } else {
                IList<IClientHolder> slaves = controller.SlavesHolder;

                var account = parameters["accountid"];
                int count = slaves.Where((c) => c.Account == account).Count();

                if (count == 0) {
                    var slave = new ClientHolder(account);
                    slave.Account = account;
                    slaves.Add(slave);
                }

                if (masterDatas.Count > 0) {
                    foreach (var item in masterDatas) {
                        response.Append(item.ToString());
                    }
                }else {
                    response.Append("[EMPTY]");
                }
            }

            controller.RefreshUISlaves();


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
            if (maintenace) {
                Logger.debug("Server Maintenace !");
                p.write503();
                p.outputStream.WriteLine("503 Server Temporarily Unavailable");
                return;
            }
            

           

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
}