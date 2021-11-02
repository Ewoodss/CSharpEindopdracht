using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AdminGui.Models;
using AdminGui.Util;
using Framework.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing
{
    [TestClass]
    public class AdminTest
    {
        private static ThreadSafeObservableList<Process> processList = new()
        {
            Dispatcher = Dispatcher.CurrentDispatcher
        };
        

        [TestMethod]

        public void NoDuplicateProcesses()
        {

            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);
            Process process2 = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            Task.Delay(1000).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 1);
        }

        [TestMethod]
        public void NoDuplicateProcesses2()
        {
            ThreadSafeObservableList<Process> processList = new ThreadSafeObservableList<Process>();
            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);
            Process process2 = new Process
            {
                Name = "lololo",
                PID = 10,
                SessionName = "lololo",
                SessionNumber = 1,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            Task.Delay(1000).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 1);
        }

        [TestMethod]
        public void Processes1()
        {
            ThreadSafeObservableList<Process> processList = new ThreadSafeObservableList<Process>();
            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);
            Process process2 = new Process
            {
                Name = "lololo",
                PID = 12,
                SessionName = "lololo",
                SessionNumber = 0,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            Task.Delay(1000).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 2);
        }
    }
}