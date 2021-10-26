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

namespace AdminGui.ViewModels
{
    public class ClientViewModel : ObservableObject
    {
        private ClientList clients;
        private Client selectedClient = null;

        public ClientViewModel()
        {
            Process process = new Process() { Name = "Hallo", MemoryUsage = 10.2, PID = 10, SessionName = "Ewout", SessionNumber = 69 };
            this.clients = new ClientList();
            this.Clients.Add(new Client() { IPAdress = "Luuk", Processes = new ObservableCollection<Process>() { process, process } });
            this.Clients.Add(new Client() { IPAdress = "Twan", Processes = new ObservableCollection<Process>() { process, process } });
            this.Clients.Add(new Client() { IPAdress = "Ewout", Processes = new ObservableCollection<Process>() { process, process } });
        }

        public ClientList Clients { get => clients; private set => clients = value; }

        public Client SelectedClient
        {
            get { return this.selectedClient; }
            set
            {
                if (this.selectedClient != value)
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

        private ICommand sleepCommand;
        public ICommand SleepCommand
        {
            get
            {
                if(sleepCommand == null)
                {
                    sleepCommand = new RelayCommand(x => 
                    {
                        MessageBox.Show("Hallo");
                    }, 
                    x => true);
                }
                return sleepCommand;
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

        public void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.NotifyPropertyChanged("GetSelectedCombinedSoftware");
        }
    }
}