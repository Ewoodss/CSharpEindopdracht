using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientHandler : Connection
    {
        public AdminHandler(Server server, TcpClient client) : base(server, client)
        {

        }
    }
}