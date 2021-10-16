using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Framework;

namespace Client
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class TaskManagerActions
    {
        public TaskManagerActions(Actions actions, Connection connection)
        {
            actions.AddAction("GetTotalCpuUsage", GetTotalCpuUsage);
        }

        async Task<double> GetCpuLoadAsync(Process process)
        {
            TimeSpan startCpuTime = process.TotalProcessorTime;
            Stopwatch timer = Stopwatch.StartNew();

            await Task.Delay(100);

            TimeSpan endCpuTime = process.TotalProcessorTime;
            timer.Stop();

            return (endCpuTime - startCpuTime).TotalMilliseconds /
                   (Environment.ProcessorCount * timer.ElapsedMilliseconds);
        }

        public List<string> Test()
        {
            PerformanceCounter mem = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "Working Set - Private",
                ReadOnly = true
            };
            PerformanceCounter cpu = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "% Processor Time",
                ReadOnly = true
            };
            List<string> henk = new List<string>();

            foreach (Process process in Process.GetProcesses())
            {
                string processProcessName = process.ProcessName;

                mem.InstanceName = processProcessName;
                cpu.InstanceName = processProcessName;

                dynamic task = new ExpandoObject();
                task.cpuUsages = cpu.NextValue() / Environment.ProcessorCount;
                task.memsize =mem.NextValue()/ 1048576;
                task.name = processProcessName;
                henk.Add(processProcessName+" "+task.cpuUsages + " " + task.memsize);

            }

            
            mem.Close();
            mem.Dispose();
            return henk;
        }


        private bool GetTotalCpuUsage(RequestData<dynamic> request)
        {
            float currentCpuTotalUsage = CurrentCpuTotalUsage();
            request.Data = currentCpuTotalUsage;
            return currentCpuTotalUsage > 0.0f;
        }


        private float CurrentCpuTotalUsage()
        {
            using PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpuCounter.NextValue();
        }
    }
#pragma warning restore CA1416 // Validate platform compatibility
}