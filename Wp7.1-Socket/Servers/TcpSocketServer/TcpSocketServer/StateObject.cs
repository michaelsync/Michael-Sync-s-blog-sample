
using System.Net.Sockets;
using System.Text;

namespace TcpSocketServer
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        public string DeviceName;
        // Client  socket.
        public Socket WorkSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] Buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder StrBuilder = new StringBuilder();
    }
}
