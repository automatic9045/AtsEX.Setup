using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup
{
    internal class ProcessEnumerator
    {
        public IReadOnlyList<ProcessInfo> GetProcesses()
        {
            using (ManagementClass win32Process = new ManagementClass("Win32_Process"))
            {
                using (ManagementObjectCollection win32ProcessInstances = win32Process.GetInstances())
                {
                    List<ProcessInfo> processes = new List<ProcessInfo>();
                    foreach (ManagementObject obj in win32ProcessInstances)
                    {
                        ProcessInfo processInfo = null;
                        try
                        {
                            processInfo = new ProcessInfo((uint)obj["ProcessId"], (string)obj["Name"], (string)obj["ExecutablePath"]);
                        }
                        catch { }

                        if (processInfo is null) continue;

                        processes.Add(processInfo);
                    }

                    return processes;
                }
            }
        }
    }
}
