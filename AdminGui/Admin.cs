using System.Net.Sockets;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;

        public Admin(string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5001);
            Connection connection = new Connection(tcpClient);

        }
    }
}