namespace TextOrder.Holder {
    public interface IClientHolder {
        
        string Name {
            get;
            set;
        }

        ClientData Contents {
            get;
            set;
        }
        
    }
}