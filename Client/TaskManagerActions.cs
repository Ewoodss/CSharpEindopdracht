using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Framework;

namespace Client
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class TaskManagerActions
    {
        private readonly string localIp;

        public TaskManagerActions(Actions actions, Connection connection)
        {
            localIp = connection.GetLocalIp();
            actions.AddAction("GetTotalCpuUsage", GetTotalCpuUsage);
            actions.AddAction("GetTotalMemoryUsage", GetTotalMemoryUsage);
            actions.AddAction("GetTotalDiskUsage", GetTotalDiskUsage);
            actions.AddAction("GetTotalGpuUsage", GetTotalGpuUsage);
            actions.AddAction("GetRunningProcesses", GetRunningProcesses);
        }

        private bool GetRunningProcesses(RequestData<dynamic> request)
        {
            List<Framework.Models.Process> currentRunningProcesses = CurrentRunningProcesses();
            request.Data = currentRunningProcesses;
            request.Origin = localIp;
            return currentRunningProcesses.Count > 1;
        }

        private bool GetTotalCpuUsage(RequestData<dynamic> request)
        {
            float usage = CurrentCpuTotalUsage();
            request.Data = usage;
            request.Origin = localIp;
            return usage > 0.0f;
        }

        private bool GetTotalMemoryUsage(RequestData<dynamic> request)
        {
            float usage = CurrentMemoryTotalUsage();
            request.Data = usage;
            request.Origin = localIp;
            return usage > 0.0f;
        }

        private bool GetTotalDiskUsage(RequestData<dynamic> request)
        {
            float usage = CurrentDiskTotalUsage();
            request.Data = usage;
            request.Origin = localIp;
            return usage > 0.0f;
        }

        private bool GetTotalGpuUsage(RequestData<dynamic> request)
        {
            Task<float> task = CurrentGpuTotalUsage();
            task.Wait();
            float usage = task.Result;
            request.Data = usage;
            request.Origin = localIp;
            return usage > 0.0f;
        }

        public List<Framework.Models.Process> CurrentRunningProcesses()
        {
            List<Framework.Models.Process> processesList = new List<Framework.Models.Process>();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                Framework.Models.Process modelProcess = new Framework.Models.Process();
                modelProcess.Name = process.MainWindowTitle;
                modelProcess.MemoryUsage = process.WorkingSet64;
                modelProcess.PID = process.Id;
                if (modelProcess.Name.Length < 1)
                {
                    modelProcess.Name = process.ProcessName;
                }

                modelProcess.SessionName = process.ProcessName;
                modelProcess.SessionNumber = process.SessionId;
                processesList.Add(modelProcess);
            }

            return processesList;
        }

        static async Task<float> CurrentGpuTotalUsage()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var counterNames = category.GetInstanceNames();
                var gpuCounters = new List<PerformanceCounter>();
                var result = 0f;

                foreach (string counterName in counterNames)
                {
                    if (counterName.EndsWith("engtype_3D"))
                    {
                        foreach (PerformanceCounter counter in category.GetCounters(counterName))
                        {
                            if (counter.CounterName == "Utilization Percentage")
                            {
                                gpuCounters.Add(counter);
                            }
                        }
                    }
                }

                gpuCounters.ForEach(x => { _ = x.NextValue(); });

                await Task.Delay(1000);

                gpuCounters.ForEach(x => { result += x.NextValue(); });

                return result;
            }
            catch
            {
                return 0f;
            }
        }

        static readonly PerformanceCounter
            DiskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

        private static float CurrentDiskTotalUsage()
        {
            return DiskCounter.NextValue();
        }

        static readonly PerformanceCounter CpuCounter =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

        private static float CurrentCpuTotalUsage()
        {
            return CpuCounter.NextValue();
        }

        static readonly PerformanceCounter MemCounter =
            new PerformanceCounter("Memory", "% Committed Bytes In Use", null);

        private static float CurrentMemoryTotalUsage()
        {
            return MemCounter.NextValue();
        }
    }


#pragma warning restore CA1416 // Validate platform compatibility
}