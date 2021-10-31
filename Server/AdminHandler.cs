using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Framework;

namespace Server
{
    public class AdminHandler : IHandler
    {
        public ServerConnection connection { get; set; }
        public AdminHandler(TcpClient client, ConnectionsManager connectionsManager)
        {
            Actions actions = new Actions();
            this.connection = new ServerConnection(client, connectionsManager);
            AdminActions adminActions = new AdminActions(actions,connection, connectionsManager);
            this.connection.actions = actions;
        }
        public void Start()
        {
            connection.Start();
        }






    }
}