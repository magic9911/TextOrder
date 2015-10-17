using GoldStar.Lib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

// offered to the public domain for any use with no restriction
// and also with no warranty of any kind, please enjoy. - David Jeske.

// simple HTTP explanation
// http://www.jmarshall.com/easy/http/

namespace YuriNET.CoreServer.Http {

    /// <summary>
    /// Http Processor
    /// </summary>
    public class HttpProcessor {
        private readonly Logger Logger = Logger.getInstance();

        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();
        public IDictionary<String, String> http_query;

        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv) {
            this.socket = s;
            this.srv = srv;
        }

        private string streamReadLine(Stream inputStream) {
            int next_char;
            StringBuilder data = new StringBuilder();
            while (srv.isActive()) {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') {
                    break;
                }
                if (next_char == '\r') {
                    continue;
                }
                if (next_char == -1) {
                    Thread.Sleep(1);
                    continue;
                };
                data.Append(Convert.ToChar(next_char));
            }
            return data.ToString();
        }

        public void process() {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET")) {
                    handleGETRequest();
                } else if (http_method.Equals("POST")) {
                    handlePOSTRequest();
                }
            } catch (Exception e) {
                Logger.error("Exception: " + e.ToString());
                write404();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null;
            outputStream = null; // bs = null;
            socket.Close();
            if (!srv.isActive()) {
                Logger.info("Closed connection.");
            }
        }

        public void parseRequest() {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3) {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_query = ToUrlCollection(http_url);
            http_protocol_versionstring = tokens[2];

            Logger.debug("starting: " + request);
        }

        private IDictionary<String, String> ToUrlCollection(string http_url) {
            IDictionary<String, String> ht = new Dictionary<String, String>();

            Uri uri = new Uri("http://server" + http_url);
            http_url = uri.Query.Substring(uri.Query.IndexOf('?') + 1);
            var queries = http_url.Split('&');

            for (int i = 0, count = queries.Length; i < count; i++) {
                var kvp = queries[i].Split('=');
                String key = null;
                String value = null;
                if (null != kvp) {
                    key = HttpUtility.UrlDecode(kvp[0], Encoding.UTF8);
                    if (kvp.Length > 1)
                        value = HttpUtility.UrlDecode(kvp[1], Encoding.UTF8);
                    ht.Add(key, value);
                }
            }
            return ht;
        }

        public void readHeaders() {
            Logger.debug("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null) {
                if (line.Equals("")) {
                    Logger.debug("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1) {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' ')) {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Logger.debug("header: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest() {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;

        public void handlePOSTRequest() {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream
            // we hand him needs to let him see the "end of the stream" at this content
            // length, because otherwise he won't know when he's seen it all!

            Logger.debug("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.Contains("Content-Length")) {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE) {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0) {
                    Logger.debug("starting Read, to_read={0}", to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    Logger.debug("read finished, numread={0}", numread);
                    if (numread == 0) {
                        if (to_read == 0) {
                            break;
                        } else {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Logger.debug("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));
        }

        public void writeSuccess(string content_type = "text/html") {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write400() {
            outputStream.WriteLine("HTTP/1.0 400 Bad Request");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write401() {
            outputStream.WriteLine("HTTP/1.0 401 Unauthorized");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write403() {
            outputStream.WriteLine("HTTP/1.0 403 Forbidden");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write404() {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write500() {
            outputStream.WriteLine("HTTP/1.0 500 Internal Server Error");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void write503() {
            outputStream.WriteLine("HTTP/1.0 503 Service Unavailable");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }
}