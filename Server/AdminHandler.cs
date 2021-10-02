using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class AdminHandler : Connection
    {
        public AdminHandler(NetworkStream networkStream) : base(networkStream)
        {
        }
    }
}