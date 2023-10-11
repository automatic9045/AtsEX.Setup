using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using IWshRuntimeLibrary;

namespace AtsEx.Setup.Installing
{
    internal class StartMenu : IDisposable
    {
        private readonly string StartMenuPath;
        private readonly WshShell Shell;

        public StartMenu()
        {
            StartMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");
            Shell = new WshShell();
        }

        public void CreateShortcut(string relativePath, string targetPath)
        {
            IWshShortcut shortcut = (IWshShortcut)Shell.CreateShortcut(Path.Combine(StartMenuPath, relativePath));
            shortcut.TargetPath = targetPath;

            shortcut.Save();
            Marshal.FinalReleaseComObject(shortcut);
        }

        public void Dispose()
        {
            Marshal.FinalReleaseComObject(Shell);
        }
    }
}
