using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

using BveEx.Setup.Models;

namespace BveEx.Setup.Installing
{
    internal static class Permission
    {
        public static bool TryElevate()
        {
            string arguments = "n"
                + CreateOptionText("--bve6", TargetPath.Bve6Path.Value)
                + CreateOptionText("--bve5", TargetPath.Bve5Path.Value)
                + CreateOptionText("--scenario", TargetPath.ScenarioDirectory.Value)
                + (TargetPath.InstallSdk.Value ? " --sdk" : string.Empty);

            WindowInteropHelper helper = new WindowInteropHelper(Application.Current.MainWindow);
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = Assembly.GetExecutingAssembly().Location,
                Verb = "RunAs",
                Arguments = arguments,
                ErrorDialog = true,
                ErrorDialogParentHandle = helper.Handle,
            };

            try
            {
                Process.Start(processStartInfo);
                return true;
            }
            catch (Win32Exception ex)
            {
                switch (ex.NativeErrorCode)
                {
                    case 1223:
                        return false;
                    default:
                        throw;
                }
            }


            string CreateOptionText(string key, InstallationTarget target)
                => $" {key} \"{target.Path}\"{(target != InstallationTarget.NotIdentified && target.HasInstalled ? $" {key}-for-log" : string.Empty)}";
        }
    }
}
