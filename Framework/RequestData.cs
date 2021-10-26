using System;
using Newtonsoft.Json;

namespace Framework
{
    public class RequestData<TDataType>
    {
        public RequestData()
        {
        }

        public RequestData(string action, TDataType data)
        {
            Action = action;
            Data = data;
        }

        [JsonProperty] public string Action { get; set; }

        [JsonProperty] public string Status { get; set; }
        [JsonProperty] public TDataType Data { get; set; }

        [JsonProperty] public string Origin { get; set; }




        public override string ToString()
        {
            return $"_action: {Action},_data: {Data}";
        }
    }
}