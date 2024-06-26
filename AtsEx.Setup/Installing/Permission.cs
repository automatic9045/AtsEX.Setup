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

using AtsEx.Setup.Models;

namespace AtsEx.Setup.Installing
{
    internal static class Permission
    {
        public static bool TryElevate()
        {
            WindowInteropHelper helper = new WindowInteropHelper(Application.Current.MainWindow);
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = Assembly.GetExecutingAssembly().Location,
                Verb = "RunAs",
                Arguments = "n" + CreateOptionText("-6", TargetPath.Bve6Path.Value.Path) + CreateOptionText("-5", TargetPath.Bve5Path.Value.Path) + CreateOptionText("-s", TargetPath.ScenarioDirectory.Value.Path),
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


            string CreateOptionText(string key, string value) => value is null ? "" : $" {key} \"{value}\"";
        }
    }
}
