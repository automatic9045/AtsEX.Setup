using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup.Releases
{
    internal static class BveExVersion
    {
        public static Version Current { get; }
        public static Version Latest { get; private set; }

        static BveExVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Current = assembly.GetName().Version;
        }

        public static async Task CommunicateAsync()
        {
            BveExRepositoryHost repositoryHost = new BveExRepositoryHost();
            ReleaseInfo release = await repositoryHost.GetLatestReleaseAsync();
            Latest = release.Version;
        }
    }
}
