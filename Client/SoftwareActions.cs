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

        public SoftwareActions(Actions actions, Connection connection)
        {
            actions.AddAction("List software", ListSoftware);
            localIp = connection.GetLocalIp();
        }

        private bool ListSoftware(RequestData<dynamic> request)
        {
            RequestData<dynamic> responseData = new RequestData<dynamic>();
            Dictionary<string, string> software = SoftwareSearch.Result();
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
