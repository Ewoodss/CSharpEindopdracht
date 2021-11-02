using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Client;
using Framework.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Process = System.Diagnostics.Process;

namespace Testing
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void ClientRunningProcessesTest()
        {
            bool succes = false;
            List<Framework.Models.Process> currentRunningProcesses = TaskManagerActions.CurrentRunningProcesses();
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Framework.Models.Process currentRunningProcess in currentRunningProcesses)
            {
                if (currentRunningProcess.PID == currentProcess.Id)
                {
                    succes = true;
                    break;
                }
            }

            Assert.IsTrue(succes);
        }


        [TestMethod]
        public void TcpConnectionTest()
        {
            string ip = "127.0.0.1";
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 5001);
            tcpListener.Start();
            Client.Client client = new Client.Client(ip);
            string remoteIp = client.connection.GetRemoteIp();
            Assert.IsTrue(remoteIp.Contains(ip));
            tcpListener.Stop();
        }
    }
}