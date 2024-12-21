using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup
{
    public class ProcessInfo
    {
        public uint Id { get; }
        public string Name { get; }
        public string Path { get; }

        public ProcessInfo(uint id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }
    }
}
