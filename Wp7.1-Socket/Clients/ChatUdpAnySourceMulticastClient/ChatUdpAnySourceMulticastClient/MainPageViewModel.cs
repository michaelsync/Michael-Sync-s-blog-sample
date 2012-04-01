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
using System.Collections.ObjectModel;
using Microsoft.Phone.Info;
using System.ComponentModel;


namespace ChatUdpAnySourceMulticastClient {
    public class MainPageViewModel : INotifyPropertyChanged {
        MyUdpAnySourceMulticastClient client;
        public MainPageViewModel() {
            Messages = new ObservableCollection<Message>();
            Message = new ChatUdpAnySourceMulticastClient.Message() { Type = MessageType.Self };
            SendCommand = new DelegateCommand(OnSendCommandExecuted);
            ConnectCommand = new DelegateCommand(OnConnectCommandExecuted);
        }
        void OnConnectCommandExecuted() {
            client = new MyUdpAnySourceMulticastClient();
            client.Received += new MyUdpAnySourceMulticastClient.ReceiveHandler(client_Received);
            client.JoinAsync(UserName, () => { IsChatViewVisible = true; });
        }

        void client_Received(object sender, EventArgs<Message> e) {
            Messages.Add(e.Data);
        }
        void OnSendCommandExecuted() {
            client.SendAsync(UserName, Message.Text,
                () => Message = new ChatUdpAnySourceMulticastClient.Message() { Type = MessageType.Self });

        }

        public string UserName { get; set; }        
        private Message message;

        public Message Message {
            get { return message; }
            set {
                message = value;
                this.NotifyPropertyChanged("Message");
            }
        }
        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand ConnectCommand { get; private set; }
        public ObservableCollection<Message> Messages { get; set; }
        private bool isChatViewVisible;

        public bool IsChatViewVisible {
            get {
                return isChatViewVisible;
            }
            set {
                isChatViewVisible = value;
                NotifyPropertyChanged("IsChatViewVisible");
            }
        }

        public string DeviceNameAndId {
            get {
                var deviceName = DeviceExtendedProperties.GetValue("DeviceName") as string;
                return deviceName + BitConverter.ToString(DeviceExtendedProperties.GetValue("DeviceUniqueId") as byte[]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
