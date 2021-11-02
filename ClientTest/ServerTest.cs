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
        private Server.Server server;

        [TestInitialize]
        public void AssemblyInit()
        {
            server = new Server.Server(5001, 5002);
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

        [TestCleanup]
        public void Clean()
        {
            this.server.Stop();
        }
    }
}