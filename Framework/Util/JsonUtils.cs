using Newtonsoft.Json;

namespace Framework
{
    public class JsonUtils
    {
        public static RequestData<string> deserializeStringData(string dataString)
        {
            return JsonConvert.DeserializeObject<RequestData<string>>(dataString);
        }

        public static string serializeStringData(RequestData<string> requestData)
        {
            return JsonConvert.SerializeObject(requestData);
        }

    }
}