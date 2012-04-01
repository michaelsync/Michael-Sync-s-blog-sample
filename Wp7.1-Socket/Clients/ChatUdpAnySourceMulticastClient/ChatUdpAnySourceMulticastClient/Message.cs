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
using System.ComponentModel;

namespace ChatUdpAnySourceMulticastClient {
    public enum MessageType { 
        FromOthers,
        Self,
        Notification
    }
    public class Message : INotifyPropertyChanged {
        private string text;

        public string Text {
            get { return text; }
            set { 
                text = value;
                NotifyPropertyChanged("Text");
            }
        }
        public DateTime Timestamp { get; set; }
        
        public string UserName { get; set; }
        public MessageType Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
