using System;
using System.IO;
using System.Net.Sockets;
using Framework;

namespace Server
{
    public class ServerConnection : Connection
    {
        private ConnectionsManager connectionsManager;
        public ServerConnection(TcpClient tcpClient, ConnectionsManager connectionsManager) : base(tcpClient)
        {
            this.connectionsManager = connectionsManager;
        }

        protected override void onIOException(IOException ioException)
        {
            connectionsManager.RemoveConnection(this);
        }
    }
}