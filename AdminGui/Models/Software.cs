using Framework;
using System;

namespace AdminGui.Models
{
    public class Software : ObservableObject
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Software software &&
                   Name == software.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}