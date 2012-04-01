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

namespace ChatUdpUnicast {
    public class UdpSocketClient : IDisposable {
        private const int PORT = 22222;
        private Socket socket;
        private bool disposed = false;
        private const int MAX_BUFFER_SIZE = 1024;

        public UdpSocketClient(IPAddress ipAddress) {
            IPEndPoint = new IPEndPoint(ipAddress, PORT);
            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);

        }

        public delegate void ReceiveHandler(object sender, EventArgs<Message> e);
        public event ReceiveHandler Received;
        private void RaiseReceived(Message message) {
            if (Received != null) {
                Received(this, new EventArgs<Message>(message));
            }
        }

        private void OnRecieveFrom() {
            var receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.RemoteEndPoint = this.IPEndPoint;
            receiveArgs.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);
            var strBdr = new StringBuilder();
            receiveArgs.Completed += (__, result) => {
                Message message = CreateMessage(result);
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    this.RaiseReceived(message);
                });
                socket.ReceiveFromAsync(receiveArgs);
            };
            socket.ReceiveFromAsync(receiveArgs);
        }

        private Message CreateMessage(SocketAsyncEventArgs result) {
            var package = Encoding.UTF8.GetString(result.Buffer, 0, result.BytesTransferred).Trim('\0');
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

        public void SendJoinMessageAsync(string userName, Action success) {
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

            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = this.IPEndPoint;
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += (__, e) => {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                        success();
                });
            };
            socket.SendToAsync(args);

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

        ~UdpSocketClient() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
