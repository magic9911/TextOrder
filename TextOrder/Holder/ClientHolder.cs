using System.Collections.Generic;

namespace TextOrder.Holder {
    public class ClientHolder : IClientHolder {
        private IList<ClientData> contents;

        public string Name { get; set; }
        public string Account { get; set; }

        public IList<ClientData> Contents {
            get {
                return contents;
            }

            set {
                contents = value;
            }
        }

        public ClientHolder(string name) {
            Name = name;
            contents = new List<ClientData>();
        }

        
    }
}
