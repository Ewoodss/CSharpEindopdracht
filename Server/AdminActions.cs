using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class AdminActions
    {
        private ConnectionsManager connectionsManager;
        private ServerConnection connection;

        public AdminActions(Actions actions, ServerConnection serverConnection, ConnectionsManager connectionsManager)
        {
            this.connectionsManager = connectionsManager;
            this.connection = serverConnection;
            actions.AddAction("SendToClients",SendToClients);
            actions.AddAction("GetAllClients", GetAllClients);
        }

        private bool SendToClients(RequestData<dynamic> requestData)
        {
            List<string> clientIps = null;
            RequestData<dynamic> requestDataForClient = null;
            if (requestData is {Data: JObject dataObject})
            {
                (clientIps, requestDataForClient) = dataObject.ToObject<(List<string>, RequestData<dynamic>)>();
            }

            if (clientIps == null || requestDataForClient == null)
            {
                return false;
            }

            requestDataForClient.Origin = connection.GetRemoteIp();
            List<ClientHandler> ClientHandlers = connectionsManager.ClientHandlers;

            foreach (var clientHandler in clientIps.SelectMany(ip => ClientHandlers.Where(clientHandler => clientHandler.connection.GetRemoteIp().Equals(ip))))
            {
                clientHandler.connection.SendString(JsonUtils.SerializeStringData(requestDataForClient));
            }

            return true;
        }

        




        private bool GetAllClients(RequestData<dynamic> notNeeded)
        {
            List<string> ips = new List<string>();

            connectionsManager.ClientHandlers.ForEach(s1=> ips.Add(s1.connection.GetRemoteIp()));

            RequestData<List<string>> requestData = new(action: "AddClients", data: ips);
            string serializeStringData = JsonUtils.SerializeStringData(requestData);
            if (serializeStringData != null) connection.SendString(serializeStringData).Wait();
            return ips.Count > 0;
        }



    }
}