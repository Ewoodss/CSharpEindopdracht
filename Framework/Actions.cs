using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework
{
    public class Actions
    {
        public delegate bool ActionHandler<TDataType>(RequestData<TDataType> request);

        private Dictionary<string, ActionHandler<dynamic>> actions;

        public Actions()
        {
            actions = new Dictionary<string, ActionHandler<dynamic>>();
            
            actions.Add("GetActions", (response) =>
            {
                response.Data = GetActions();
                return response.Data != null;
            });
        }

        public void AddAction(string actionCommand, ActionHandler<dynamic> action)
        {
            actions.Add(actionCommand, action);
        }

        private ActionHandler<object> GetAction(string actionCommand)
        {
            return actions[actionCommand];
        }

        private List<string> GetActions()
        {
            return new List<string>(this.actions.Keys);
        }

        public async Task DoAction(string textData, IConnection connection)
        {
            RequestData<object> requestData = JsonUtils.deserializeStringData(textData);

            ActionHandler<object> actionHandler = GetAction(requestData.Action);
            bool succes = actionHandler.Invoke( requestData);

            requestData.Status = succes ? "succes" : "failed";
            Console.WriteLine(requestData);
            await connection.SendString(JsonUtils.serializeStringData(requestData));
        }
    }
}