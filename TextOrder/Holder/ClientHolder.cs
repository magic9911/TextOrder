namespace TextOrder.Holder {
    public class ClientHolder : IClientHolder {
        private ClientData contents;

        public string Name { get; set; }

        public ClientData Contents {
            get {
                return contents;
            }

            set {
                contents = value;
            }
        }

        public ClientHolder(string name) {
            Name = name;
        }

        
    }
}
