using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Newtonsoft.Json;

namespace Server
{
    public class ConnectionsManager
    {
        public List<AdminHandler> AdminHandlers { get; private set; }
        public List<ClientHandler> ClientHandlers { get; private set; }

        public ConnectionsManager()
        {
            this.AdminHandlers = new List<AdminHandler>();
            this.ClientHandlers = new List<ClientHandler>();
        }

        public void AddAdminHandler(AdminHandler adminHandler)
        {
            if (this.AdminHandlers.Contains(adminHandler))
                return;
            this.AdminHandlers.Add(adminHandler);
        }

        public void AddClientHandler(ClientHandler clientHandler)
        {
            if (this.ClientHandlers.Contains(clientHandler))
                return;
            this.ClientHandlers.Add(clientHandler);
        }

        public void RemoveConnection(Connection connection)
        {
            ClientHandlers.RemoveAll((s1) => s1.connection.Equals(connection));
            AdminHandlers.RemoveAll((s1) => s1.connection.Equals(connection));
        }




    }
}