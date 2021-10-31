using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(5001, 5002);
            server.Start();
            //Console.ReadKey();
            Task.Delay(-1).Wait();
        }
    }
}
