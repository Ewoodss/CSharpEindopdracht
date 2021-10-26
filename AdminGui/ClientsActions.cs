using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdminGui
{
    public class ClientsActions
    {
        private ViewModels.ClientViewModel clientViewModel;

        public ClientsActions(Actions actions, ViewModels.ClientViewModel clientViewModel)
        {
            actions.AddAction("AddClient", AddClient);
            actions.AddAction("AddClients", AddClients);
            actions.AddAction("RemoveClient", RemoveClient);
            this.clientViewModel = clientViewModel;
        }

        private bool AddClient(string clientIp)
        {
            if (clientIp == null)
            {
                return false;
            }
            this.clientViewModel.Clients.Add(clientIp);
            return true;
        }

        private bool AddClient(RequestData<dynamic> requestData)
        {
            return AddClient(requestData.Data as string);
        }

        private bool RemoveClient(RequestData<dynamic> requestData)
        {
            string clientIp = requestData.Data as string;

            if (clientIp == null)
            {
                return false;
            }
            this.clientViewModel.Clients.Remove(clientIp);
            return true;
        }

        private bool AddClients(RequestData<dynamic> requestData)
        {
            JArray jArray = requestData.Data;
            foreach (JToken jToken in jArray)
            {
                AddClient((string)jToken);
            }
            return true;
        }
    }
}