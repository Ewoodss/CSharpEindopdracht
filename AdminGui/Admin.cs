using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Documents;
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
            ClientsActions clientsActions = new ClientsActions(actions,connection);
            this.connection.actions = actions;
        }

        public void Start()
        {
            connection.Start();
            GetClients();

            Task.Delay(-1).Wait();
        }

        public void GetClients()
        {
            connection.SendString(JsonUtils.SerializeStringData(new RequestData<object>(action: "GetAllClients",data:""))).Wait();
        }

        //example
        // public void test()
        // {
        //     List<string> list = new List<string>();
        //     RequestData<dynamic> requestData = new RequestData<dynamic>();
        //     requestData.Action = "Lock";
        //     sendToClients(list, requestData).Wait();
        // }


        


    }
}