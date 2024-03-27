using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.Releases
{
    internal static class AtsExVersion
    {
        public static Version Current { get; }
        public static Version Latest { get; private set; }

        static AtsExVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Current = assembly.GetName().Version;
        }

        public static async Task CommunicateAsync()
        {
            AtsExRepositoryHost repositoryHost = new AtsExRepositoryHost();
            ReleaseInfo release = await repositoryHost.GetLatestReleaseAsync();
            Latest = release.Version;
        }
    }
}
