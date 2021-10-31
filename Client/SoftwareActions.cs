using Contracts;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SoftwareActions
    {
        private readonly string localIp;
        private Dictionary<string, string> software { get; set; }

        public SoftwareActions(Actions actions, Connection connection)
        {
            actions.AddAction("List software", ListSoftware);
            actions.AddAction("StartSoftware", StartSoftware);
            localIp = connection.GetLocalIp();
        }

        private bool StartSoftware(RequestData<object> request)
        {
            if (request.Data is string program)
            {
                if (software.ContainsKey(program))
                {
                    string s = software[program];
                    System.Diagnostics.Process.Start(s);
                    return true;
                }
                
            }

            return false;
        }

        private bool ListSoftware(RequestData<dynamic> request)
        {
            RequestData<dynamic> responseData = new RequestData<dynamic>();
            if (software == null)
            {
                Dictionary<string, string> dictionary = SoftwareSearch.Result();
                software = dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            }

            responseData.Action = "List software";
            responseData.Data = software.Select(x => new SoftwareRequestItem { Name = x.Key, Path = x.Value}).ToList();
            responseData.Origin = localIp;

            (string, RequestData<dynamic>) data = (request.Origin, responseData);
            request.Action = "SendToAdmin";
            request.Data = data;
            request.Origin = localIp;

            return software.Count > 1;
        }
    }
}
