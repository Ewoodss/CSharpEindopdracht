using AdminGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Documents;
using AdminGui.Models;
using Framework;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;
        private ClientViewModel clientViewModel;
        private Actions actions;

        public Admin(ClientViewModel clientView, string address = "localhost")
        {
            actions = new Actions();
            TcpClient tcpClient = new TcpClient(address, 5002);
            this.connection = new Connection(tcpClient);
            this.clientViewModel = clientView;
            ClientsActions clientsActions = new ClientsActions(actions, clientViewModel);
            this.connection.actions = actions;
        }

        public void Start()
        {
            connection.Start();
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


    }
}