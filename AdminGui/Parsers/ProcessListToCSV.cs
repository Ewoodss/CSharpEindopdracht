using AdminGui.Models;
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
        public static string Parse(ProcessList processList)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("Name,PID,Memory Usage,Session name,Session number");

            foreach (Process process in processList.Processes)
            {
                result.AppendLine($"{process.Name},{process.PID},{process.MemoryUsage},{process.SessionName},{process.SessionNumber}");
            }

            return result.ToString();
        }
    }
}
