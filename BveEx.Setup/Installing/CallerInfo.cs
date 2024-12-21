using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup.Installing
{
    internal class CallerInfo
    {
        private static readonly Version MinCompatibleVersion = new Version(1, 0, 31106, 1);

        public static readonly string FileName = "BveEx.Caller.InputDevice.dll";

        public AssemblyName Source { get; }
        public bool CanUseSimpleInstall { get; }

        public CallerInfo(AssemblyName source)
        {
            Source = source;
            CanUseSimpleInstall = MinCompatibleVersion <= Source.Version;
        }

        public static CallerInfo Create(string callerPath)
        {
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(callerPath);
            return new CallerInfo(assemblyName);
        }

        public static CallerInfo CreateFromBvePath(string bvePath)
        {
            string callerPath = Path.Combine(Path.GetDirectoryName(bvePath), "Input Devices", FileName);
            return Create(callerPath);
        }

        public static CallerInfo TryCreateFromBvePath(string bvePath)
        {
            try
            {
                return CreateFromBvePath(bvePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
