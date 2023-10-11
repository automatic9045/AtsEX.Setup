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

        private static readonly Task<ReleaseInfo> GetLatestReleaseTask;
        public static Version Latest => GetLatestReleaseTask.Result.Version;

        static AtsExVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Current = assembly.GetName().Version;

            AtsExRepositoryHost repositoryHost = new AtsExRepositoryHost();
            GetLatestReleaseTask = repositoryHost.GetLatestReleaseAsync();
        }
    }
}
