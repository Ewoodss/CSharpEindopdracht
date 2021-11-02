using System.Net.Sockets;
using System.Security.Permissions;
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
        private Dispatcher dispatcher;
        private ThreadSafeObservableList<Process> processList;

        [TestInitialize]
        public void Start()
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
            processList = new(this.dispatcher);
        }
        
        [TestMethod]
        public void NoDuplicateProcesses()
        {
            this.processList.Clear();
            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);

            this.DoActions();

            Process process2 = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            DoActions();
            Task.Delay(100).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 1);
        }

        [TestMethod]
        public void NoDuplicateProcesses2()
        {
            this.processList.Clear();
            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);

            DoActions();

            Process process2 = new Process
            {
                Name = "lololo",
                PID = 10,
                SessionName = "lololo",
                SessionNumber = 1,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            DoActions();
            Task.Delay(100).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 1);
        }

        [TestMethod]
        public void Processes1()
        {
            this.processList.Clear();
            Process process = new Process
            {
                Name = "henk",
                PID = 10,
                SessionName = "henk",
                SessionNumber = 1,
                MemoryUsage = 243576
            };

            processList.Add(process);

            DoActions();

            Process process2 = new Process
            {
                Name = "lololo",
                PID = 12,
                SessionName = "lololo",
                SessionNumber = 0,
                MemoryUsage = 243576
            };
            processList.Add(process2);
            DoActions();
            Task.Delay(100).Wait();

            int processesCount = processList.Items.Count;
            Assert.IsTrue(processesCount == 2);
        }

        private void DoActions()
        {
            DispatcherFrame frame = new DispatcherFrame();
            this.dispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}