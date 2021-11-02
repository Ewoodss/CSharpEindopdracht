using System;

namespace Framework.Models
{
    public class Process : ObservableObject
    {
        public string Name { get; set; }
        public int PID { get; set; }
        public string SessionName { get; set; }
        public int SessionNumber { get; set; }
        public double MemoryUsage { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(PID, SessionNumber);
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType() && this.PID == (obj as Process).PID && this.SessionNumber == (obj as Process).SessionNumber;
        }
    }
}
