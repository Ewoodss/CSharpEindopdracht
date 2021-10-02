using System.Net.Sockets;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;

        public Admin(string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5002);
            Connection connection = new Connection(tcpClient);
        }

        public void start()
        {
            connection.Start();
        }

    }
}