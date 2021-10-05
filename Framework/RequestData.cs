using System;
using Newtonsoft.Json;

namespace Framework
{
    public class RequestData<TDataType>
    {
        [JsonProperty] 
        public string Action { get; set; }

        [JsonProperty]
        public string Status { get; set; }
        [JsonProperty]
        public TDataType Data { get; set; }




        public override string ToString()
        {
            return $"_action: {Action},_data: {Data}";
        }
    }
}