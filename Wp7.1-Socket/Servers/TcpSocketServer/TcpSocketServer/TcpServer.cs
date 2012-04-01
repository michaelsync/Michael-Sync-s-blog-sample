using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using ChatTcpUnicast;


namespace TcpSocketServer {
    public class TcpServer {
        public static ManualResetEvent manualReset = new ManualResetEvent(false);
        static ConcurrentBag<StateObject> stateObjects = new ConcurrentBag<StateObject>();
        const int PORT = 22222;
        const int MAX_PENDING_REQUEST = 10;
        static Socket listener;

        public static void Start() {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            Listen();
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
            Console.Clear();
            Console.WriteLine("One player exits from the game");
            stateObjects = new ConcurrentBag<StateObject>();
            WaitForConnections();
        }

        private static void Listen() {
            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            
            try {
                listener.Bind(new IPEndPoint(IPAddress.Any, PORT));
                listener.Listen(MAX_PENDING_REQUEST);
                WaitForConnections();

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private static void WaitForConnections() {
            while (true) {
                manualReset.Reset();
                Console.WriteLine("Waiting for a connection...{0}", GetLocalIpAddress());
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                manualReset.WaitOne();
            }
        }

        private static IPEndPoint GetLocalIpAddress() {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var localIpAddress = new IPEndPoint(ipHostInfo.AddressList
                                        .Where(item => item.AddressFamily == AddressFamily.InterNetwork)
                                        .First(), PORT);
            return localIpAddress;
        }

        public static void AcceptCallback(IAsyncResult ar) {
            manualReset.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar) {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;
            
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0) {
                var rawMessage = Encoding.UTF8.GetString(state.Buffer, 0, bytesRead);
                var messages = rawMessage.Split(';');

                if (messages.Length > 1) {
                    var command = messages[0];
                    var deviceName = messages[1];
                    Console.WriteLine("Command '{0}' is received from Device Name '{1}'", command, deviceName);
                    switch (command) {
                        case SocketCommands.CONNECT:
                            state.DeviceName = deviceName;
                            stateObjects.Add(state);
                            Send(rawMessage);
                            break;
                        default:
                            Send(rawMessage);
                            break;
                    }
                }

                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                                   new AsyncCallback(ReadCallback), state);


            }
        }

        private static void Send(String data) {
            foreach (var socket in stateObjects) {
                var msgArray = data.Split(';');
                var command = msgArray[0];
                var deviceName = msgArray[1];                
                Send(socket.WorkSocket, data);                
            }
        }

        private static void Send(Socket handler, String data) {
            if (handler.Connected) {
                Console.WriteLine("Sending {0}.", data);
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                handler.SendBufferSize = byteData.Length;
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
        }

        private static void SendCallback(IAsyncResult ar) {
            try {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

    }


}
