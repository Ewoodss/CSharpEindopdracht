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
        private int clientPort;
        private int adminPort;

        private TcpListener clientTcpListner;
        private TcpListener adminTcpListner;

        public Server(string host, int clientPort, int adminPort)
        {
            this.host = host;
            this.clientPort = clientPort;
            this.adminPort = adminPort;
            this.ConnectionsManager = new ConnectionsManager();
        }

        public ConnectionsManager ConnectionsManager { get; set; }

        public void Start()
        {
            this.clientTcpListner = new TcpListener(IPAddress.Parse(host), this.clientPort);
            this.adminTcpListner = new TcpListener(IPAddress.Parse(host), this.adminPort);

            this.clientTcpListner.Start();
            this.adminTcpListner.Start();

            this.clientTcpListner.BeginAcceptTcpClient(new AsyncCallback(onClientConnect), null);
            Console.WriteLine($"Started listining for clients on {host}:{clientPort}");

            this.adminTcpListner.BeginAcceptTcpClient(new AsyncCallback(onAdminConnect), null);
            Console.WriteLine($"Started listining for clients on {host}:{clientPort}");
        }

        private void onClientConnect(IAsyncResult ar)
        {
            TcpClient tcpClient = this.clientTcpListner.EndAcceptTcpClient(ar);

            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");

            ClientHandler connection = new ClientHandler(this, tcpClient);
            connection.Start();
            this.ConnectionsManager.AddConnection(connection);

            this.clientTcpListner.BeginAcceptTcpClient(new AsyncCallback(onClientConnect), null);
        }

        private void onAdminConnect(IAsyncResult ar)
        {
            TcpClient tcpClient = this.clientTcpListner.EndAcceptTcpClient(ar);

            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");

            AdminHandler connection = new AdminHandler(this, tcpClient);
            connection.Start();
            this.ConnectionsManager.AddConnection(connection);

            this.clientTcpListner.BeginAcceptTcpClient(new AsyncCallback(onClientConnect), null);
        }

    }
}