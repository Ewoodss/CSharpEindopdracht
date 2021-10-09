using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminGui.Models
{
    public class Client : ObservableObject
    {
        public string IPAdress { get; set; }

        private ObservableCollection<Process> processes;

        public Client()
        {
            this.processes = new ObservableCollection<Process>();
        }

        public ObservableCollection<Process> Processes
        {
            get { return this.processes; }
            set
            {
                this.processes = value;
                NotifyPropertyChanged();
            }
        }
    }
}
