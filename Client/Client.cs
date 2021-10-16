using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Framework;

namespace Client
{
    public class Client
    {
        private Connection connection;
        private string uuid;
        private Actions actions;

        public Client(string address = "localhost")
        {
            actions = new Actions();
            TcpClient tcpClient = new TcpClient(address, 5001);
            this.connection = new Connection(tcpClient);
            
            connection.actions = actions;
            
        }

        public void Start()
        {
            connection.Start();
            //dit ziet er niet uit als de juiste plek voor dit.
            PowerActions powerActions = new PowerActions(actions);
        }

    }
}