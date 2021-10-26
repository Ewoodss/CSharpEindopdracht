namespace Framework.Models
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
