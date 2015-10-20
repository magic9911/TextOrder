using System.Collections.Generic;

namespace TextOrder.Holder {
    public class ClientHolder : IClientHolder {
        private DictData contents;

        public string Name { get; set; }
        public string Account { get; set; }

        public DictData Contents {
            get {
                return contents;
            }

            set {
                contents = value;
            }
        }

        public ClientHolder() : this("") {
        }

        public ClientHolder(string name) {
            Name = name;
            contents = new DictData();
        }

        
    }
}
