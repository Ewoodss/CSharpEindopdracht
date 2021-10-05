using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;

namespace Client
{
    public class Actions
    {
        public delegate bool ActionHandler<T>(Connection connection, RequestData<T> request);

        private Dictionary<string, ActionHandler<string>> actions;

        public Actions()
        {
            actions = new Dictionary<string, ActionHandler<string>>();
            PowerActions powerActions = new PowerActions(this);

            actions.Add("hello", (connection, request) =>
            {
                request.SetData("hello back");
                connection.SendString(JsonUtils.serializeStringData(request));
                return true;
            });
        }

        public void AddAction(string actionCommand, ActionHandler<string> action)
        {
            actions.Add(actionCommand, action);
        }

        private ActionHandler<string> GetAction(string actionCommand)
        {
            return actions[actionCommand];
        }

        private List<string> GetCommands()
        {
            return new List<string>(this.actions.Keys);
        }

        public async Task DoAction(string textData, Connection connection)
        {
            RequestData<string> deserializeStringData = JsonUtils.deserializeStringData(textData);

            ActionHandler<string> actionHandler = GetAction(deserializeStringData.GetAction());
            actionHandler.Invoke(connection, deserializeStringData);

        }
    }
}