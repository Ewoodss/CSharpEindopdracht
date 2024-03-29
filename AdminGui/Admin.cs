using AdminGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Documents;
using AdminGui.Models;
using Framework;
using Contracts;
using Framework.Models;

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
            Task.Delay(-1).Wait();
        }

        public void GetClients()
        {
            connection.SendString(JsonUtils.SerializeStringData(new RequestData<object>(action: "GetAllClients",data:"heogaboega"))).Wait();
        }

        public async void GetProcceses(List<Client> clients)
        {
            RequestData<dynamic> requestData = new RequestData<dynamic>();
            requestData.Action = "GetRunningProcesses";
            await SendToClients(clients, requestData);
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
            await this.SendToClients(clients, data);
        }

        
        public async void SendLock(List<Client> clients)
        {
            RequestData<object> data = new RequestData<object>()
            {
                Action = "Lock"
            };
            await this.SendToClients<object>(clients, data);
        }

        public async void SendSleep(List<Client> clients)
        {
            RequestData<object> data = new RequestData<object>()
            {
                Action = "Sleep"
            };
            await this.SendToClients<object>(clients, data);
        }

        public async void SendShutdown(List<Client> clients)
        {
            RequestData<object> data = new RequestData<object>()
            {
                Action = "Shutdown"
            };
            await this.SendToClients<object>(clients, data);
        }

        public async void SendLogOff(List<Client> clients)
        {
            RequestData<object> data = new RequestData<object>()
            {
                Action = "LogOff"
            };
            await this.SendToClients<object>(clients, data);
        }

        public async void KillProcess(Client client, Process process)
        {
            RequestData<KillRunnningProcessCommand> data = new RequestData<KillRunnningProcessCommand>()
            {
                Action = "KillRunningProcess",
                Data = new KillRunnningProcessCommand()
                {
                    PID = process.PID
                }
            };
            await this.SendToClients(new List<Client>() {client}, data);
        }

        public async void GetSoftware(List<Client> selectedClients)
        {
            RequestData<dynamic> requestData = new RequestData<dynamic>();
            requestData.Action = "List software";
            await SendToClients(selectedClients, requestData);
        }
        public async void StartSoftware(List<Client> selectedClients, string programName)
        {
            RequestData<dynamic> requestData = new RequestData<dynamic>();
            requestData.Action = "StartSoftware";
            requestData.Data = programName;
            await SendToClients(selectedClients, requestData);
        }


        private async Task SendToClients<TData>(List<Client> clients, RequestData<TData> requestData)
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