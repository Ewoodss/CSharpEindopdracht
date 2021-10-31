using Contracts;
using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ChatMessageActions
    {
        public ChatMessageActions(Actions actions)
        {
            actions.AddAction("Chat", x => OnChat(x.Data));
        }

        private static bool OnChat(JObject message)
        {
            ChatMessageCommand command = message.ToObject<ChatMessageCommand>();
            MessageBox((IntPtr)0, command.Message, "Chat message", 0);
            return true;
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr h, string message, string title, int type);
    }
}
