using Framework.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Connection
    {
        private Server server;
        private TcpClient client;
        private NetworkStream networkStream;

        private byte[] totalBuffer = new byte[0];
        private byte[] buffer = new byte[1024];

        public Connection(Server server, TcpClient client)
        {
            this.server = server;
            this.client = client;
        }

        public void Start()
        {
            this.networkStream = this.client.GetStream();
            this.networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(onRead), null);
        }

        private async Task send(byte[] bytes,byte type)
        { 
            await this.networkStream.WriteAsync(new byte[] {type});
            byte[] buffelength = BitConverter.GetBytes(bytes.Length);
            await this.networkStream.WriteAsync(buffelength,0,buffelength.Length);
            await this.networkStream.WriteAsync(bytes, 0, bytes.Length);
            await this.networkStream.FlushAsync();
        }
        public async Task SendString(string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            await this.send(encodedText, 1);
        }

        private void onRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = this.networkStream.EndRead(ar);
                totalBuffer = ArrayUtils.Concat(totalBuffer, buffer, receivedBytes);
            }
            catch (IOException)
            {
                this.server.ConnectionsManager.RemoveConnection(this);
                return;
            }

            while (totalBuffer.Length >= 4)
            {
                byte packetType = totalBuffer[0];
                int packetLength = BitConverter.ToInt32(totalBuffer, 1);
                if (totalBuffer.Length >= packetLength + 5)
                {
                    this.onResponse(packetType, totalBuffer.GetSubArray(5, packetLength));

                    totalBuffer = totalBuffer.GetSubArray(5 + packetLength, totalBuffer.Length - packetLength - 5);
                }
                else
                    break;
            }
            this.networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(onRead), null);
        }

        protected virtual void onResponse(byte type, byte[] data)
        {
            if (type == 1)
            {
                string textData = Encoding.UTF8.GetString(totalBuffer, 5, data.Length);
                Console.WriteLine(textData);
            }
        }

    }
}