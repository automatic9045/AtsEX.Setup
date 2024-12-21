using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup
{
    internal class InstallationTarget
    {
        public static readonly InstallationTarget NotIdentified = new InstallationTarget(null, true);

        public string Path { get; }
        public bool HasInstalled { get; private set; } = false;

        public InstallationTarget(string path, bool hasInstalled = false)
        {
            Path = path;
            HasInstalled = hasInstalled;
        }

        public void MarkAsInstalled()
        {
            HasInstalled = true;
        }
    }
}
