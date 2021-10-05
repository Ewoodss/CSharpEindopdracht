using System.Net.Sockets;
using System.Threading.Tasks;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;
        private ClientViewModel clientViewModel;

        public Admin(ClientViewModel clientView, string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5002);
            this.connection = new Connection(tcpClient);
            this.clientViewModel = clientView;
        }

        public void start()
        {
            connection.Start();
            //Task.Delay(-1).Wait();
        }

    }
}