using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IConnection
    {
        void Start();
        
        Task SendString(string text);

    }
}