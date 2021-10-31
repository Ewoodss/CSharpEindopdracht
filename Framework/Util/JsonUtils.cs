using Newtonsoft.Json;

namespace Framework
{
    public class JsonUtils
    {
        public static RequestData<object> DeserializeStringData(string dataString)
        {
            return JsonConvert.DeserializeObject<RequestData<object>>(dataString);
        }

        public static string SerializeStringData(RequestData<object> requestData)
        {
            return JsonConvert.SerializeObject(requestData);
        }

        public static string SerializeStringData<TDataType>(RequestData<TDataType> requestData)
        {
            return JsonConvert.SerializeObject(requestData);
        }

    }
}