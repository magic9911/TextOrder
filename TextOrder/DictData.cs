using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextOrder.Holder;

namespace TextOrder {
    public class DictData : Dictionary<string, string> {
        public void Add(string id,string positions) {
            base.Add(id, positions);
        }
    }
}
