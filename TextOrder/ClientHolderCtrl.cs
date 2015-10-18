using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TextOrder.Holder;

namespace TextOrder {
    public partial class ClientHolderCtrl : UserControl {
        
        public delegate void ClosingControlDelegate(object sender, IClientHolder client);
        public event ClosingControlDelegate ClosingControl;
        public IClientHolder Client;

        [DefaultValue(true)]
        public bool Closeable {
            get {
                return btnClose.Visible;
            }
            set {
                btnClose.Visible = value;
            }
        }

        public ClientHolderCtrl() {
            InitializeComponent();
            if(null == Client) {
                Client = new ClientHolder("");
            }
        }

        public ClientHolderCtrl(IClientHolder client) : this() {
            this.Client = client;
            Refresh();
        }

        public override void Refresh() {
            if (InvokeRequired) {
                Invoke(new Action(() => Refresh()));
                return;
            }
            //base.Refresh();

            txtName.Text = Client.Account;
        }

        private void btnClose_Click(object sender, EventArgs e) {
            if (null != ClosingControl) {
                ClosingControl(this, Client);
            }
            Dispose();
        }

        

        private void txtName_TextChanged(object sender, EventArgs e) {
            Client.Account = txtName.Text;
        }


    }
}
