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
using System.Net.Sockets;
using System.Text;
using Microsoft.Phone.Info;
using System.Globalization;

namespace ChatUdpAnySourceMulticastClient {
    public class MyUdpAnySourceMulticastClient : IDisposable {
        /// <summary>
        /// The IP address of the multicast group. 
        /// </summary>
        /// <remarks>
        /// A multicast group is defined by a multicast group address, which is an IP address 
        /// that must be in the range from 224.0.0.0 to 239.255.255.255. Multicast addresses in 
        /// the range from 224.0.0.0 to 224.0.0.255 inclusive are “well-known” reserved multicast 
        /// addresses. For example, 224.0.0.0 is the Base address, 224.0.0.1 is the multicast group 
        /// address that represents all systems on the same physical network, and 224.0.0.2 represents 
        /// all routers on the same physical network.The Internet Assigned Numbers Authority (IANA) is 
        /// responsible for this list of reserved addresses. For more information on the reserved 
        /// address assignments, please see the IANA website.
        /// http://go.microsoft.com/fwlink/?LinkId=221630
        /// </remarks>
        private const string GROUP_ADDRESS = "224.0.0.1";

        /// <summary>
        /// This defines the port number through which all communication with the multicast group will take place. 
        /// </summary>
        /// <remarks>
        /// The value in this example is arbitrary and you are free to choose your own.
        /// </remarks>
        private const int GROUP_PORT = 54329;
        
        private UdpAnySourceMulticastClient socket;
        private bool disposed = false;
        private const int MAX_BUFFER_SIZE =  2048;

        public MyUdpAnySourceMulticastClient() {
            socket = new UdpAnySourceMulticastClient(IPAddress.Parse(GROUP_ADDRESS), GROUP_PORT);
        }

        public delegate void ReceiveHandler(object sender, EventArgs<Message> e);
        public event ReceiveHandler Received;
        private void RaiseReceived(Message message) {
            if (Received != null) {
                Received(this, new EventArgs<Message>(message));
            }
        }

        private void OnRecieveFrom() {
            var buffer = new byte[MAX_BUFFER_SIZE];
            socket.BeginReceiveFromGroup(buffer, 0, buffer.Length, BeginReceiveFromGroupCallBack,
                buffer);
        }
        private void BeginReceiveFromGroupCallBack(IAsyncResult ar) {
            IPEndPoint source;
            this.socket.EndReceiveFromGroup(ar, out source);
            var buffer = ar.AsyncState as byte[];
            Message message = CreateMessage(buffer);
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                this.RaiseReceived(message);
                OnRecieveFrom();
            });
        }
        private Message CreateMessage(byte[] result) {
            var package = Encoding.UTF8.GetString(result, 0, result.Length).Trim('\0');
            if (!string.IsNullOrEmpty(package)) {
                var messageArray = package.Split(';');
                var commandName = messageArray[0];
                var deviceName = messageArray[1];
                var userName = messageArray[2];
                var userMessage = string.Format("{0} has joined.", userName);
                MessageType type = MessageType.Notification;
                if (messageArray.Length > 3) {
                    userMessage = messageArray[3];
                    type = deviceName == DeviceNameAndId ? MessageType.Self : MessageType.FromOthers;
                }
                DateTime messageSentTimeStamp = DateTime.Now;
                if (messageArray.Length > 4) {
                    messageSentTimeStamp = DateTime.Parse(messageArray[4], CultureInfo.InvariantCulture);
                }

                var message = new Message() {
                    UserName = userName, Text = userMessage,
                    Type = type, Timestamp = messageSentTimeStamp
                };
                return message;
            }
            return null;
        }

        public void JoinAsync(string userName, Action success) {
            this.socket.BeginJoinGroup(ar => {
                this.socket.EndJoinGroup(ar);
                SendJoinMessageAsync(userName, success);
            }, null);            
        }

        private void SendJoinMessageAsync(string userName, Action success) {
            var message = string.Format("{0};{1};{2}", SocketCommands.CONNECT, this.DeviceNameAndId, userName);
            SendAsync(message, () => {
                OnRecieveFrom();
                success();
            });
        }

        public void SendAsync(string userName, string message, Action success) {
            var formattedMessage = string.Format("{0};{1};{2};{3};{4}",
                    SocketCommands.TEXT, this.DeviceNameAndId, userName, message, DateTime.Now);
            SendAsync(formattedMessage, success);
        }
        private void SendAsync(string message, Action success) {

            var buffer = Encoding.UTF8.GetBytes(message);
            socket.BeginSendToGroup(buffer, 0, buffer.Length, (ar) => {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    if (ar.IsCompleted) {
                        success();
                    }
                }
                );
            }, null);

        }

        public string DeviceNameAndId {
            get {
                var deviceName = DeviceExtendedProperties.GetValue("DeviceName") as string;
                return deviceName + BitConverter.ToString(DeviceExtendedProperties.GetValue("DeviceUniqueId") as byte[]);
            }
        }

        public IPEndPoint IPEndPoint { get; private set; }

        #region Dispose
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    this.socket.Dispose();
                }
                disposed = true;
            }
        }

        ~MyUdpAnySourceMulticastClient() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
