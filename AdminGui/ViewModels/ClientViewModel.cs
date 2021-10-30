using AdminGui.Commands;
using AdminGui.Models;
using Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;
using Framework.Models;
using AdminGui.Views;
using Microsoft.Win32;
using System.IO;
using AdminGui.Util;

namespace AdminGui.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        //private ClientList clients;
        private ThreadSafeObservableList<Client> clients;
        private Client selectedClient = null;
        private Software selectedSoftware = null;
        private readonly Admin admin;

        public ClientViewModel(Admin admin)
        {
            this.clients = new ThreadSafeObservableList<Client>();
            this.admin = admin;
           
        }

        public ThreadSafeObservableList<Client> Clients { get => clients; private set => clients = value; }

        public Client SelectedClient
        {
            get { return this.selectedClient; }
            set
            {
                if (!Equals(this.selectedClient, value))
                {
                    if (this.selectedClient == null)
                    {
                        this.selectedClient = new Client();
                    }
                    this.selectedClient = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Software SelectedSoftware
        {
            get { return this.selectedSoftware; }
            set
            {
                if(!Equals(this.selectedSoftware, value))
                {
                    if (this.selectedSoftware == null)
                    {
                        this.selectedSoftware = new Software();
                    }
                    this.selectedSoftware = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Software> GetSelectedCombinedSoftware
        {
            get { return this.clients.Items.Where(x => x.IsSelected).Select(x => x.Softwares).SelectMany(x => x.Items).Distinct().ToList(); }
        }

        private ICommand chatCommand;
        public ICommand ChatCommand
        {
            get
            {
                if(chatCommand == null)
                {
                    chatCommand = new RelayCommand(x =>
                    {
                        InputDialog inputDialog = new InputDialog("Please enter your name:", "John Doe");
                        if (!inputDialog.ShowDialog().Value)
                            return;
                            

                        List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
                        this.admin.SendChatMessage(selectedClients, inputDialog.Answer);
                    },
                    x => true);
                }
                return chatCommand;
            }
        }

        private ICommand sleepCommand;
        public ICommand SleepCommand
        {
            get
            {
                if(sleepCommand == null)
                {
                    sleepCommand = new RelayCommand(x => 
                    {
                        List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
                        this.admin.SendSleep(selectedClients);
                    }, 
                    x => true);
                }
                return sleepCommand;
            }
        }

        private ICommand lockCommand;
        public ICommand LockCommand
        {
            get
            {
                if(lockCommand == null)
                {
                    lockCommand = new RelayCommand(x => 
                    {
                        List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
                        this.admin.SendLock(selectedClients);
                    }, x => true);
                }
                return lockCommand;
            }
        }

        private ICommand shutDownCommand;
        public ICommand ShutdownCommand
        {
            get 
            {
                if(shutDownCommand == null)
                {
                    shutDownCommand = new RelayCommand(x => 
                    {
                        List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
                        this.admin.SendShutdown(selectedClients);
                    }, x => true);
                }
                return shutDownCommand;
            }
        }

        private ICommand logOffCommand;
        public ICommand LogOffCommand
        {
            get
            {
                if (logOffCommand == null)
                {
                    logOffCommand = new RelayCommand(x =>
                    {
                        List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
                        this.admin.SendLogOff(selectedClients);
                    }, x => true);
                }
                return logOffCommand;
            }
        }


        private ICommand startSoftwareCommand;

        public ICommand StartSoftwareCommand
        {
            get
            {
                if (startSoftwareCommand == null)
                {
                    startSoftwareCommand = new RelayCommand(x =>
                    {
                        MessageBox.Show("startSoftwareCommand");
                    },
                    x => true);
                }
                return startSoftwareCommand;
            }
        }

        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                if (exportCommand == null)
                {
                    exportCommand = new RelayCommand(x =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Csv file (*.csv)|*.csv";
                        if (!saveFileDialog.ShowDialog().Value)
                            return;

                        string csvValue = AdminGui.Parsers.ProcessListToCSV.Parse(this.selectedClient.Processes);
                        File.WriteAllText(saveFileDialog.FileName, csvValue);
                    },
                    x => true);
                }
                return exportCommand;
            }
        }

        private ICommand killProcessCommand;
        public ICommand KillProcessCommand
        {
            get
            {
                if (killProcessCommand == null)
                {
                    killProcessCommand = new RelayCommand(x =>
                    {
                        this.admin.KillProcess(this.SelectedClient, (x as Process));
                    },
                    x => true);
                }
                return killProcessCommand;
            }
        }

        public void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Client> selectedClients = Clients.Items.Where(x => x.IsSelected).ToList();
            this.admin.GetProcceses(selectedClients);
            this.admin.GetSoftware(selectedClients);
            this.NotifyPropertyChanged("GetSelectedCombinedSoftware");
        }
    }
}