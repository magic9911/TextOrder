using GoldStar.Lib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace YuriNET.CoreServer.Http {

    internal class HttpController : HttpServer {
        private readonly Logger Logger = Logger.getInstance();

        //private ConcurrentDictionary<short, Client> clients = new ConcurrentDictionary<short, Client>();
        //private IProducerConsumerCollection<short> pool;
        //private Server myServer;

        private Thread runnerThread;

        private bool maintenace = false;
        private int timeout = 30;
        //private int maxClients = 90;
        //private int peekClients = 0;
        //private string password = null;

        public HttpController(int port)
            : base(port) {
        }

        //public HttpController(Server server)
        //    : base(server.SocketPort) {
        //    myServer = server;
        //    timeout = server.Timeout;
        //    //maxClients = server.MaxClients;
        //    Logger.info("Timeout : {0} secs", timeout);
        //    //Logger.info("Max Client : {0}", maxClients);

        //    //initClients();

        //    Logger.info("Creating Timeout Kicker Thread...");
        //    runnerThread = new Thread(runner);
        //    runnerThread.Start();
        //}

        //private void initClients() {
        //    Logger.info("Allocating Client Pool...");
        //    // Clear
        //    clients.Clear();

        //    IList<short> allShort = new List<short>();
        //    for (short i = short.MinValue; i < short.MaxValue; i++) {
        //        allShort.Add(i);
        //    }
        //    allShort.Shuffle();
        //    pool = new ConcurrentQueue<short>(allShort);

        //    Logger.info("Took 0 secs to initialize pool.");
        //}

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
            var segments = p.http_url;
            var parameters = p.http_query;
            
            
            // Condition
            if (maintenace) {
                Logger.debug("Server Maintenace !");
                p.write503();
                p.outputStream.WriteLine("503 Server Temporarily Unavailable");
                return;
            }
            

            // Response text
            StringBuilder response = new StringBuilder();
            response.Append(@"<h1>Hello World</h1>");
            

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