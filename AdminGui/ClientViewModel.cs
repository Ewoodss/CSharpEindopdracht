using Framework;
using System.Collections.ObjectModel;

namespace AdminGui
{
    public class ClientViewModel : ObservableObject
    {
        private ObservableCollection<Client> clients;
        private Client selectedClient = null;

        public ClientViewModel()
        {
            Process process = new Process() { Name = "Hallo", MemoryUsage = 10.2, PID = 10, SessionName = "Ewout", SessionNumber = 69 };
            this.clients = new ObservableCollection<Client>()
            {
                new Client(){ IPAdress="Luuk", Processes = new ObservableCollection<Process>(){ process, process } },
                new Client(){ IPAdress="Luuk", Processes = new ObservableCollection<Process>(){ process } },
                new Client(){ IPAdress="Luuk" }
            };
        }

        public ObservableCollection<Client> Clients
        {
            get { return this.clients; }
            set {
                this.clients = value;
                NotifyPropertyChanged();
            }
        }

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
    }
}