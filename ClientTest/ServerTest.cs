using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace Testing
{
    [TestClass]
    public class ServerTest
    {
        static Server.Server server = new Server.Server( 5001, 5002);


        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            server.Start();
        }

        [TestMethod]
        public void ServerClientConnectionTest()
        {
            TcpClient tcpClientClient = new TcpClient("127.0.0.1", 5001);
            Assert.IsTrue(tcpClientClient.Connected);
        }

        [TestMethod]
        public void ServerAdminConnectionTest()
        {
            TcpClient tcpAdminClient = new TcpClient("127.0.0.1", 5002);
            Assert.IsTrue(tcpAdminClient.Connected);
        }
    }
}