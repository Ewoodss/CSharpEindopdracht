using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            //base.OnStartup(e);
            MainWindow window = new MainWindow();
            ClientViewModel VM = new ClientViewModel();
            Admin admin = new Admin(VM);
            Thread thread = new Thread(new ThreadStart(admin.Start));
            thread.Start();
            
            window.DataContext = VM;
            window.Show();
        }
    }
}
