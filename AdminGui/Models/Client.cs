using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Models;

namespace AdminGui.Models
{
    public class Client : ObservableObject
    {
        public string IPAdress { get; set; }

        private ObservableCollection<Process> processes;
        private ObservableCollection<Software> softwares;
        private bool isSelected = false;

        public Client()
        {
            this.processes = new ObservableCollection<Process>();
            this.softwares = new ObservableCollection<Software>();
            this.softwares.Add(new Software() { Name = "Luuk" });
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

        public ObservableCollection<Software> Softwares
        {
            get { return this.softwares; }
            private set
            {
                this.softwares = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSelected { get { return this.isSelected; } set { this.isSelected = value; } }

        public override bool Equals(object obj)
        {
            return obj is Client client &&
                   IPAdress == client.IPAdress;
        }

        public override string ToString()
        {
            return IPAdress;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IPAdress);
        }
    }
}
