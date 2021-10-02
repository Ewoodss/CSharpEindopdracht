using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Connection
    {
        private NetworkStream networkStream;

        public Connection(NetworkStream networkStream)
        {
            this.networkStream = networkStream;
        }

        private async Task send(byte[] bytes,byte type)
        { 
            await this.networkStream.WriteAsync(new byte[] {type});
            byte[] buffelength = BitConverter.GetBytes(bytes.Length);
            await this.networkStream.WriteAsync(buffelength,0,buffelength.Length);
            await this.networkStream.WriteAsync(bytes, 0, bytes.Length);
            await this.networkStream.FlushAsync();
        }






    }
}