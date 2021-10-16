using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminGui.Models
{
    public class Process : ObservableObject
    {
        public string Name { get; set; }
        public int PID { get; set; }
        public string SessionName { get; set; }
        public int SessionNumber { get; set; }
        public double MemoryUsage { get; set; }
    }
}
