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

namespace AdminGui.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        private ClientList clients;
        private Client selectedClient = null;
        private readonly Admin admin;

        public ClientViewModel(Admin admin)
        {
            this.clients = new ClientList();
            this.admin = admin;
        }

        public ClientList Clients { get => clients; private set => clients = value; }

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

        public List<Software> GetSelectedCombinedSoftware
        {
            get { return this.clients.Clients.Where(x => x.IsSelected).SelectMany(x => x.Softwares).Distinct().ToList(); }
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
                            

                        List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
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
                        List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
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
                        List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
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
                        List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
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
                        List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
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
                if(exportCommand == null)
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

        public void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.NotifyPropertyChanged("GetSelectedCombinedSoftware");
            List<Client> selectedClients = Clients.Clients.Where(x => x.IsSelected).ToList();
            this.admin.GetProcceses(selectedClients);
        }
    }
}