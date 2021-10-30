using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Models;
using AdminGui.Util;

namespace AdminGui.Models
{
    public class Client : ObservableObject
    {
        public string IPAdress { get; set; }

        private ThreadSafeObservableList<Process> processes;
        private ThreadSafeObservableList<Software> softwares;
        private bool isSelected = false;

        public Client()
        {
            this.processes = new ThreadSafeObservableList<Process>();
            this.softwares = new ThreadSafeObservableList<Software>();
        }


        public ThreadSafeObservableList<Process> Processes
        {
            get { return this.processes; }
        }

        public ThreadSafeObservableList<Software> Softwares
        {
            get { return this.softwares; }
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
