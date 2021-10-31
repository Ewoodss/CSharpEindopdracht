using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AdminGui.Models;
using AdminGui.Util;
using Contracts;
using Framework;
using Framework.Models;
using Newtonsoft.Json.Linq;

namespace AdminGui
{
    public class ClientsActions
    {
        private ViewModels.ClientViewModel clientViewModel;

        public ClientsActions(Actions actions, ViewModels.ClientViewModel clientViewModel)
        {
            actions.AddAction("AddClient", AddClient);
            actions.AddAction("AddClients", AddClients);
            actions.AddAction("RemoveClient", RemoveClient);
            actions.AddAction("AddRunningProcesses", AddRunningProcesses);
            actions.AddAction("List software", ListSoftware);
            this.clientViewModel = clientViewModel;
        }

        private bool ListSoftware(RequestData<object> request)
        {
            List<SoftwareRequestItem> softwareResult = null;

            if (request is { Data: JArray dataObject })
            {
                softwareResult = dataObject.ToObject<List<SoftwareRequestItem>>();
            }
            else
            {
                return false;
            }

            string IpAddress = request.Origin.Split(':').FirstOrDefault();

            ThreadSafeObservableList<Software> softwares = this.clientViewModel.Clients.Items.FirstOrDefault(x => x.IPAdress == IpAddress).Softwares;
            softwares.Clear();

            foreach (SoftwareRequestItem software in softwareResult)
            {
                softwares.Add(new Software() { Name = software.Name});
            }

            return softwares.Items.Count > 1;
        }

        private bool AddRunningProcesses(RequestData<object> request)
        {
            Console.WriteLine();
            string IpAddress = request.Origin.Split(':').FirstOrDefault();
            ThreadSafeObservableList<Process> clientProcesses = clientViewModel.Clients.Items.FirstOrDefault(x => x.IPAdress == IpAddress).Processes;
            clientProcesses.Clear();
            
            List<Process> processes = null;

            if (request is { Data: JArray dataObject })
            {
                processes = dataObject.ToObject<List<Process>>();
            }

            if (processes == null)
            {
                var test = request.Data.GetType().ToString();
                return false;
            }

            processes.ForEach(clientProcesses.Add);

            return clientProcesses.Items.Count > 1;
        }

        private bool AddClient(string clientIp)
        {
            if (clientIp == null)
            {
                return false;
            }
            this.clientViewModel.Clients.Add(new Client()
            {
                IPAdress = clientIp
            });
            return true;
        }

        private bool AddClient(RequestData<dynamic> requestData)
        {
            return AddClient(requestData.Data as string);
        }

        private bool RemoveClient(RequestData<dynamic> requestData)
        {
            string clientIp = requestData.Data as string;

            if (clientIp == null)
            {
                return false;
            }
            this.clientViewModel.Clients.Remove(this.clientViewModel.Clients.Items.FirstOrDefault(x => x.IPAdress == clientIp));
            return true;
        }

        private bool AddClients(RequestData<dynamic> requestData)
        {
            JArray jArray = requestData.Data;
            foreach (JToken jToken in jArray)
            {
                AddClient((string)jToken);
            }
            return true;
        }
    }
}