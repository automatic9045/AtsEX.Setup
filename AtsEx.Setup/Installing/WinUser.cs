using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.Installing
{
    internal class WinUser
    {
        public string Name { get; }
        public string UserRootDirectory { get; }
        public string DocumentDirectory => Path.Combine(UserRootDirectory, "Documents");

        public WinUser(string name, string userRootDirectory)
        {
            Name = name;
            UserRootDirectory = userRootDirectory;
        }
    }
}
