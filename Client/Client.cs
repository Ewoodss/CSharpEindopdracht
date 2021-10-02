using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        private Connection connection;
        private string uuid;


        public Client(string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5001);
            this.connection = new Connection(tcpClient);
        }

        public void Start()
        {
            connection.Start();
        }


    }
}