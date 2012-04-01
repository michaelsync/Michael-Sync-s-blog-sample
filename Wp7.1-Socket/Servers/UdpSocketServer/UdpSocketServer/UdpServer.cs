using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using ChatTcpUnicast;


namespace UdpSocketServer {
    public class UdpServer {
        public static ManualResetEvent manualReset = new ManualResetEvent(false);
        static ConcurrentBag<EndPoint> remoteEndpoints = new ConcurrentBag<EndPoint>();
        const int PORT = 22222;
        const int MAX_PENDING_REQUEST = 10;
        static Socket listener;

        public static void Start() {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            listener.Bind(new IPEndPoint(IPAddress.Any, PORT));

            Console.WriteLine("Waiting for a connection...{0}", GetLocalIpAddress());

            while (true) {

                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                StateObject state = new StateObject();
                state.WorkSocket = listener;
                listener.ReceiveFrom(state.Buffer, ref remoteEndPoint);
                var rawMessage = Encoding.UTF8.GetString(state.Buffer);
                var messages = rawMessage.Split(';');

                if (messages.Length > 1) {
                    var command = messages[0];
                    var deviceName = messages[1];
                    Console.WriteLine("Command '{0}' is received from Device Name '{1}'", command, deviceName);
                    switch (command) {
                        case SocketCommands.CONNECT:
                            state.DeviceName = deviceName;
                            remoteEndpoints.Add(remoteEndPoint);
                            Send(rawMessage);
                            break;
                        default:
                            Send(rawMessage);
                            break;
                    }
                }
            }
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
            Console.Clear();
            Console.WriteLine("One player exits from the game");

        }
        private static IPEndPoint GetLocalIpAddress() {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var localIpAddress = new IPEndPoint(ipHostInfo.AddressList
                                        .Where(item => item.AddressFamily == AddressFamily.InterNetwork)
                                        .First(), PORT);
            return localIpAddress;
        }



        private static void Send(String data) {
            foreach (var endpoint in remoteEndpoints) {
                var msgArray = data.Split(';');
                var command = msgArray[0];
                var deviceName = msgArray[1];                
                Send(endpoint, data);
            }
        }

        private static void Send(EndPoint endPoint, String data) {
            Console.WriteLine("Sending {0}.", data);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            listener.SendBufferSize = byteData.Length;
            listener.SendTo(byteData, endPoint);
        }
    }
}



