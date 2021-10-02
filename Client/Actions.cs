using System;
using System.Collections.Generic;

namespace Client
{
    public class Actions
    {


        private Dictionary<string, Predicate<string>> actions;


        public Actions()
        {
            actions = new Dictionary<string, Predicate<string>>();
            PowerActions powerActions = new PowerActions(this);

        }

        public void AddAction(string actionCommand, Predicate<string> action)
        {
            actions.Add(actionCommand, action);
        }

        public Predicate<string> GetAction(string actionCommand)
        {
            return actions[actionCommand];
        }

    }
}