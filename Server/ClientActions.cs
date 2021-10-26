using System.Collections.Generic;
using System.Linq;
using Framework;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class ClientActions
    {
        private ConnectionsManager connectionsManager;
        private ServerConnection connection;

        public ClientActions(Actions actions, ServerConnection serverConnection, ConnectionsManager connectionsManager)
        {
            this.connectionsManager = connectionsManager;
            this.connection = serverConnection;
            actions.AddAction("SendToAdmin", SendToAdmin);
        }

        private bool SendToAdmin(RequestData<object> requestData)
        {
            string adminIp = null;
            RequestData<dynamic> requestDataForAdmin = null;
            if (requestData is { Data: JObject dataObject })
            {
                (adminIp, requestDataForAdmin) = dataObject.ToObject<(string, RequestData<dynamic>)>();
            }

            if (adminIp == null || requestDataForAdmin == null)
            {
                return false;
            }

            requestDataForAdmin.Origin = connection.GetRemoteIp();
            List<AdminHandler> adminHandlers = connectionsManager.AdminHandlers;

            foreach (AdminHandler adminHandler in adminHandlers)
            {
                if (adminHandler.connection.GetRemoteIp().Equals(adminIp) )
                {
                    adminHandler.connection.SendString(JsonUtils.SerializeStringData(requestDataForAdmin));
                }
            }

            return true;
        }
    }
}