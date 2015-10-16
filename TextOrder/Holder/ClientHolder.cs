namespace TextOrder.Holder {
    public class ClientHolder : IClientHolder {
        private string contents;

        public string Name { get; set; }
        public string FilePath { get; set; }

        public string Contents {
            get {
                return contents;
            }

            set {
                contents = value;
            }
        }

        public ClientHolder(string name, string path) {
            Name = name;
            FilePath = path;
        }

        public bool Read() {
            contents = FileUtility.ReadFile(FilePath);
            return true;
        }

        public bool Write() {
            return Write(contents);
        }

        public bool Write(string contents) {
            this.contents = contents;
            FileUtility.WriteFile(FilePath, contents);
            return true;
        }
    }
}
