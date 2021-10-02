using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        private Connection connection;
        private string uuid;


        public Client(string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5001);
            Connection connection = new Connection(tcpClient);
   

        }
    }
}