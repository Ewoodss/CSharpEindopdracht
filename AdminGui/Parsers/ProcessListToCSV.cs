using AdminGui.Models;
using AdminGui.Util;
using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminGui.Parsers
{
    public static class ProcessListToCSV
    {
        public static string Parse(ThreadSafeObservableList<Process> processList)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("Name,PID,Memory Usage,Session name,Session number");

            foreach (Process process in processList.Items)
            {
                result.AppendLine($"{process.Name},{process.PID},{process.MemoryUsage},{process.SessionName},{process.SessionNumber}");
            }

            return result.ToString();
        }
    }
}
