using System.Collections.Generic;

namespace TextOrder.Holder {
    public interface IClientHolder {
        
        string Name {
            get;
            set;
        }

        string Account {
            get;
            set;
        }

        IList<ClientData> Contents {
            get;
            set;
        }
        
    }
}