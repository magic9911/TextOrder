namespace TextOrder.Holder {
    public interface IClientHolder {
        string FilePath {
            get;
            set;
        }
        string Name {
            get;
            set;
        }

        string Contents {
            get;
            set;
        }

        bool Read();
        bool Write();
        bool Write(string contents);
    }
}