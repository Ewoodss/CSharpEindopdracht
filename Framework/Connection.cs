using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Framework.Util;

namespace Framework
{
    public class Connection : IConnection
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        public Actions actions { set; get; }

        private byte[] totalBuffer = new byte[0];
        private byte[] buffer = new byte[1024];

        public Connection(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public void Start()
        {
            this.networkStream = this.tcpClient.GetStream();
            this.networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(onRead), null);
        }

        public string GetRemoteIp() => tcpClient.Client.LocalEndPoint?.ToString();
        public string GetLocalIp() => tcpClient.Client.RemoteEndPoint?.ToString();

        public async Task SendImage(Image img)
        {
            byte[] imageBytes = ImageToBytes(img);
            await send(imageBytes, 2);
        }


        public async Task SendBytes(byte[] bytes)
        {
            await this.send(bytes, 0);
        }

        public async Task SendString(string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            await this.send(encodedText, 1);
        }

        private static byte[] ImageToBytes(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }


        private async Task send(byte[] bytes, byte type)
        {
            await this.networkStream.WriteAsync(new byte[] {type});
            byte[] bufferlength = BitConverter.GetBytes(bytes.Length);
            await this.networkStream.WriteAsync(bufferlength, 0, bufferlength.Length);
            await this.networkStream.WriteAsync(bytes, 0, bytes.Length);
            await this.networkStream.FlushAsync();
        }
        private void onRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = this.networkStream.EndRead(ar);
                totalBuffer = ArrayUtils.Concat(totalBuffer, buffer, receivedBytes);
            }
            catch (IOException e)
            {
                this.onIOException(e);
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

        protected virtual void onIOException(IOException ioException)
        {
            return;
        }


        protected virtual async void onResponse(byte type, byte[] data)
        {
            if (type == 1)
            {
                string textData = Encoding.UTF8.GetString(totalBuffer, 5, data.Length);
                Console.WriteLine(textData);
                await actions.DoAction(textData, this);
            }
        }




    }
}