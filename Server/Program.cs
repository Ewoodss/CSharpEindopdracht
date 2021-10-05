using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 5001, 5002);
            server.Start();
            //Console.ReadKey();
            Task.Delay(-1).Wait();
        }
    }
}
