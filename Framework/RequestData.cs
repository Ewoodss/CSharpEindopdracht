using Newtonsoft.Json;

namespace Framework
{
    public class RequestData<TDataType>
    {
        [JsonProperty]
        private string _action;
        [JsonProperty]
        private TDataType _data;

        public RequestData()
        {
        }

        public string GetAction()
        {
            return _action;
        }

        public void SetAction(string action)
        {
            this._action = action;
        }

        public TDataType GetData()
        {
            return _data;
        }

        public void SetData(TDataType data)
        {
            this._data = data;
        }
    }
}