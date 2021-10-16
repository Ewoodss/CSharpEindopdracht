using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class AdminActions
    {
        private ConnectionsManager connections;

        public AdminActions(Actions actions, ConnectionsManager connections)
        {
            this.connections = connections;
            actions.AddAction("SendToClients",SendToClients);
        }

        private bool SendToClients(dynamic test)
        {
            RequestData<dynamic> requestData = test as RequestData<dynamic>;
            List<string> clientIps = null;
            RequestData<dynamic> requestDataForClient = null;
            if (requestData.Data is JObject dataObject)
            {
                (clientIps, requestDataForClient) = dataObject.ToObject<(List<string>, RequestData<dynamic>)>();
            }

            if (clientIps == null || requestDataForClient == null)
            {
                return false;
            }

            List<ClientHandler> ClientHandlers = connections.ClientHandlers;

            // foreach (var clientHandler in clientIps.SelectMany(ip => ClientHandlers.Where(clientHandler => clientHandler.connection.GetIp().Equals(ip))))
            // {
            //     clientHandler.connection.SendString(JsonUtils.serializeStringData(requestDataForClient));
            // }

            foreach (ClientHandler clientHandler in ClientHandlers)
            {
                string serializeStringData = JsonUtils.serializeStringData(requestDataForClient);
                Console.WriteLine(serializeStringData);
                clientHandler.connection.SendString(serializeStringData).Wait();
            }

            return true;
        }



    }
}