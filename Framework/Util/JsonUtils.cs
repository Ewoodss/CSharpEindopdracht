using Newtonsoft.Json;

namespace Framework
{
    public class JsonUtils
    {
        public static RequestData<object> deserializeStringData(string dataString)
        {
            return JsonConvert.DeserializeObject<RequestData<object>>(dataString);
        }

        public static string serializeStringData(RequestData<object> requestData)
        {
            return JsonConvert.SerializeObject(requestData);
        }

    }
}