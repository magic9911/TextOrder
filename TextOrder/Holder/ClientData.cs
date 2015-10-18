using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextOrder.Holder {
    public class ClientData {

        public string RawData { get; private set; }

        public string Id { get; private set; }
        public string Symbol { get; private set; }
        public float Volume { get; private set; }
        public string Type { get; private set; }
        public long OpenT { get; private set; }
        public float OpenP { get; private set; }
        public float Sl { get; private set; }
        public float Tp { get; private set; }
        public long Closet { get; private set; }
        public float CloseP { get; private set; }
        public float Profit { get; private set; }
        public long Time { get; private set; }
        public string Account { get; private set; }

        public ClientData(string setData) {
            MapData(setData);
        }

        public static implicit operator ClientData(string setData) {
            return new ClientData(setData);
        }

        public void MapData(string setData) {
            RawData = setData;
            string[] fieldValues = setData.Split(';');

            int i = 0;
            Id = fieldValues[i++];
            Symbol = fieldValues[i++];
            Volume = float.Parse(fieldValues[i++]);
            Type = fieldValues[i++];
            OpenT = long.Parse(fieldValues[i++]);
            OpenP = float.Parse(fieldValues[i++]);
            Sl = float.Parse(fieldValues[i++]);
            Tp = float.Parse(fieldValues[i++]);
            Closet = long.Parse(fieldValues[i++]);
            CloseP = float.Parse(fieldValues[i++]);
            Profit = float.Parse(fieldValues[i++]);
            Time = long.Parse(fieldValues[i++]);
            Account = fieldValues[i++];
        }

        public override string ToString() {
            return RawData;
        }
    }
}
