using AdminGui.ViewModels;
using AdminGui.Views;
using Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdminGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Connection connection = new Connection(new TcpClient("localhost", 5002));

            MainWindow window = new MainWindow();
            Admin admin = new Admin(connection);
            ClientViewModel VM = new ClientViewModel(admin);
            Thread thread = new Thread(new ThreadStart(() => 
            {
                connection.Start();
                admin.Start();
            }));
            thread.Start();

            this.AddActions(connection, VM);

            window.DataContext = VM;
            window.Show();
        }

        private void AddActions(Connection connection, ClientViewModel clientViewModel)
        {
            new ClientsActions(connection.actions, clientViewModel);
        }
    }
}
