using AdminGui.ViewModels;
using System.Net.Sockets;

namespace AdminGui
{
    public class Admin
    {
        private Connection connection;
        private ClientViewModel clientViewModel;

        public Admin(ClientViewModel clientView, string address = "localhost")
        {

            TcpClient tcpClient = new TcpClient(address, 5001);
            this.connection = new Connection(tcpClient);
            this.clientViewModel = clientView;
        }

        public void start()
        {
            connection.Start();
        }

    }
}