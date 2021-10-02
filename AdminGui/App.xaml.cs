﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            Admin admin = new Admin();
            admin.start();
            ClientViewModel VM = new ClientViewModel(admin);
            window.DataContext = VM;
            window.Show();
        }
    }
}
