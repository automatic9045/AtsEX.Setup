using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace AtsEx.Setup
{
    internal class ApplicationInfo
    {
        private static readonly string RegistryName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private static readonly string Wow64RegistryName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        public static ApplicationInfo TryCreateFromRegistry(string searchDisplayName, bool isWow64)
        {
            RegistryKey parentKey = Registry.LocalMachine.OpenSubKey(isWow64 ? Wow64RegistryName : RegistryName, false);
            if (parentKey is null) return null;

            RegistryKey applicationKey = parentKey
                .GetSubKeyNames()
                .Select(name => parentKey.OpenSubKey(name))
                .FirstOrDefault(key =>
                {
                    if (key is null) return false;

                    string displayName = key.GetValue("DisplayName")?.ToString();
                    return displayName == searchDisplayName;
                });

            return null;
        }
    }
}
