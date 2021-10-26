using AdminGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Documents;
using AdminGui.Models;
using Framework;
using AdminGui.Models;
using Contracts;
using System.Linq;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;

        public Admin(Connection connection)
        {
            this.connection = connection;
        }

        public void Start()
        {
            GetClients();
            Task.Delay(1000).Wait();
            GetProcceses();
            Task.Delay(-1).Wait();
        }

        public void GetClients()
        {
            connection.SendString(JsonUtils.SerializeStringData(new RequestData<object>(action: "GetAllClients",data:"heogaboega"))).Wait();
        }

        public void GetProcceses()
        {
            List<string> list = clientViewModel.Clients.Clients.Select(x => x.IPAdress).Distinct().ToList();
            RequestData<dynamic> requestData = new RequestData<dynamic>();
            requestData.Action = "GetRunningProcesses";
            SendToClients(list, requestData).Wait();
        }


        public async Task SendToClients(List<string> clients, RequestData<dynamic> requestData)
        {
            (List<string>, RequestData<dynamic>) data = (clients, requestData);
            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "SendToClients";
            testRequestData.Data = data;
            string serializeObject = JsonUtils.SerializeStringData(testRequestData);
            //Console.WriteLine("testing: " + serializeObject);
            await connection.SendString(serializeObject);
        }

        public async void SendChatMessage(List<Client> clients, string message)
        {
            RequestData<ChatMessageCommand> data = new RequestData<ChatMessageCommand>()
            {
                Action = "Chat",
                Data = new ChatMessageCommand()
                {
                    Message = message
                }
            };
            await this.sendToClients(clients, data);
        }

        private async Task sendToClients<TData>(List<Client> clients, RequestData<TData> requestData)
        {
            List<string> clientList = clients.Select(x => x.IPAdress).ToList();
            (List<string>, RequestData<TData>) data = (clientList, requestData);
            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "SendToClients";
            testRequestData.Data = data;
            string serializeObject = JsonUtils.SerializeStringData(testRequestData);
            //Console.WriteLine("testing: " + serializeObject);
            await connection.SendString(serializeObject);
        }
    }
}