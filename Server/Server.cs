using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class Server
    {
        private string host;
        private int port;

        private TcpListener tcpListener;

        public Server(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.ConnectionsManager = new ConnectionsManager();
        }

        public ConnectionsManager ConnectionsManager { get; set; }

        public void Start()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse(host), this.port);
            this.tcpListener.Start();
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(onConnect), null);
            Console.WriteLine($"Started listining to {host}:{port}");
        }

        private void onConnect(IAsyncResult ar)
        {
            TcpClient tcpClient = this.tcpListener.EndAcceptTcpClient(ar);

            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");

            Connection connection = new Connection(this, tcpClient);
            connection.Start();
            this.ConnectionsManager.AddConnection(connection);

            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(onConnect), null);
        }
        
    }
}