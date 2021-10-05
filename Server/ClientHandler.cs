using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Server
{
    public class ClientHandler : Connection
    {
        public ClientHandler(Server server, TcpClient client) : base(server, client)
        {

        }


        public async Task GetCommands()
        {
            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "GetCommands";
            string serializeObject = JsonUtils.serializeStringData(testRequestData);
            Console.WriteLine("testing: " + serializeObject);
            await SendString(serializeObject);

        }

    }
}