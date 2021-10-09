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
            
            foreach (AdminHandler connectionsManagerAdminHandler in connectionsManager.AdminHandlers)
            {
                connectionsManagerAdminHandler.connection.SendString(JsonUtils.serializeStringData(requestData));
            }

            
        }

        public async Task GetCommands()
        {
            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "GetCommands";
            string serializeObject = JsonUtils.serializeStringData(testRequestData);
            Console.WriteLine("testing: " + serializeObject);
            await connection.SendString(serializeObject);
        }

        
    }
}