using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientHandler : Connection
    {
        public ClientHandler(TcpClient tcpClient) : base(tcpClient)
        {

        }
    }
}