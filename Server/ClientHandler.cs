using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Server
{
    public class ClientHandler : IHandler
    {
        public ServerConnection connection { get; set; }
        private ConnectionsManager connectionsManager;

        public ClientHandler(TcpClient client, ConnectionsManager connectionsManager)
        {
            this.connectionsManager = connectionsManager;
            this.connection = new ServerConnection(client, connectionsManager);
            Actions actions = new Actions();
            this.connection.actions = actions;
            new ClientActions(actions,connection,connectionsManager);
        }
        public void Start()
        {
            connection.Start();
            Announce();

        }

        public void Announce()
        {
            RequestData<object> requestData = new RequestData<object>();
            requestData.Action = "AddClient";
            requestData.Data = connection.GetRemoteIp();

            List<Task> tasks = new List<Task>();

            foreach (AdminHandler connectionsManagerAdminHandler in connectionsManager.AdminHandlers)
            {
                Task sendString = connectionsManagerAdminHandler.connection.SendString(JsonUtils.SerializeStringData(requestData));
                tasks.Add(sendString);
            }

            Task.WaitAll(tasks.ToArray());
        }


    }
}