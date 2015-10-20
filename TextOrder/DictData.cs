using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextOrder.Holder;

namespace TextOrder {
    public class DictData : Dictionary<string, ClientData> {
        public void Add(ClientData holder) {
            base.Add(holder.Account, holder);
        }
    }
}
