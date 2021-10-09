using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;

namespace AdminGui
{
    public class ClientsActions
    {
        private Connection connection;
        private List<string> clientIps;

        public ClientsActions(Actions actions,Connection connection)
        {
            clientIps = new List<string>();
            actions.AddAction("AddClient",AddClient);
            actions.AddAction("RemoveClient",AddClient);
        }

        public bool AddClient(RequestData<dynamic> requestData)
        {
            string clientIp = requestData.Data as string;

            if (clientIp == null || clientIps.Contains(clientIp))
            {
                return false;
            }

            clientIps.Add(clientIp);
            return true;
        }

        public bool RemoveClient(RequestData<dynamic> requestData)
        {
            string clientIp = requestData.Data as string;

            if (clientIp == null || !clientIps.Contains(clientIp))
            {
                return false;
            }

            clientIps.Remove(clientIp);
            return true;
        }



        public async Task sendToClients(List<string> clients, RequestData<dynamic> requestData)
        {
            (List<string>, RequestData<dynamic>) data = (clients, requestData);
            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "SendToClients";
            testRequestData.Data = data;
            string serializeObject = JsonUtils.serializeStringData(testRequestData);
            //Console.WriteLine("testing: " + serializeObject);
            await connection.SendString(serializeObject);
        }


    }
}