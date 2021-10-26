﻿using System.Collections.Generic;
using Framework;
using Newtonsoft.Json.Linq;

namespace AdminGui
{
    public class ClientsActions
    {
        private ViewModels.ClientViewModel clientViewModel;
        private List<string> clientIps;

        public ClientsActions(Actions actions, ViewModels.ClientViewModel clientViewModel)
        {
            clientIps = new List<string>();
            actions.AddAction("AddClient", AddClient);
            actions.AddAction("AddClients", AddClients);
            actions.AddAction("RemoveClient", RemoveClient);
            this.clientViewModel = clientViewModel;
        }

        private bool AddClient(string clientIp)
        {
            if (clientIp == null || clientIps.Contains(clientIp))
            {
                return false;
            }

            clientIps.Add(clientIp);
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

            if (clientIp == null || !clientIps.Contains(clientIp))
            {
                return false;
            }

            clientIps.Remove(clientIp);
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


        // public async Task sendToClients(List<string> clients, RequestData<dynamic> requestData)
        // {
        //     (List<string>, RequestData<dynamic>) data = (clients, requestData);
        //     RequestData<object> testRequestData = new RequestData<object>();
        //     testRequestData.Action = "SendToClients";
        //     testRequestData.Data = data;
        //     string serializeObject = JsonUtils.serializeStringData(testRequestData);
        //     //Console.WriteLine("testing: " + serializeObject);
        //     await connection.SendString(serializeObject);
        // }
    }
}