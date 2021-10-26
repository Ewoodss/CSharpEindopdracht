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

        private ProcessList processes;
        private SoftwareList softwares;
        private bool isSelected = false;

        public Client()
        {
            this.processes = new ProcessList();
            this.softwares = new SoftwareList();
        }


        public ProcessList Processes
        {
            get { return this.processes; }
        }

        public SoftwareList Softwares
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
