using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Framework;

namespace Client
{
    public class Actions
    {
        public delegate bool ActionHandler<T>( RequestData<T> request);

        private Dictionary<string, ActionHandler<object>> actions;

        public Actions()
        {
            actions = new Dictionary<string, ActionHandler<object>>();
            
            actions.Add("GetActions", (response) =>
            {
                response.Data = GetActions();
                return response.Data != null;
            });
        }

        public void AddAction(string actionCommand, ActionHandler<object> action)
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

            //dit kan korter en beter worden geschreven ben ff de manier kwijt
            if (succes)
            {
                requestData.Status = "succes";
            }
            else
            {
                requestData.Status = "failed";
            }

            await connection.SendString(JsonUtils.serializeStringData(requestData));
        }
    }
}