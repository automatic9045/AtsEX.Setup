using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using IWshRuntimeLibrary;

namespace BveEx.Setup.Installing
{
    internal class ShortcutFactory : IDisposable
    {
        private static readonly string StartMenuDirectory;
        private static readonly string DesktopDirectory;

        private readonly WshShell Shell;

        static ShortcutFactory()
        {
            StartMenuDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");
            DesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        public ShortcutFactory()
        {
            Shell = new WshShell();
        }

        public void CreateToDesktop(string shortcutFileName, string targetPath)
            => Create(Path.Combine(DesktopDirectory, shortcutFileName), targetPath);

        public void CreateToStartMenu(string shortcutFileName, string targetPath)
            => Create(Path.Combine(StartMenuDirectory, shortcutFileName), targetPath);

        private void Create(string shortcutPath, string targetPath)
        {
            IWshShortcut shortcut = (IWshShortcut)Shell.CreateShortcut(shortcutPath);
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
