using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ChatUdpAnySourceMulticastClient {
    public class EventArgs<T> : EventArgs {
        private T data;

        public EventArgs(T data) {
            this.data = data;
        }

        public T Data {
            get { return data; }
        }
    }
}
