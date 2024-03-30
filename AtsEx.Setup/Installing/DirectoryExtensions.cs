using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.Installing
{
    internal static class DirectoryExtensions
    {
        public static void CopyDirectory(string sourceDirectory, string destDirectory, bool recursive)
        {
            DirectoryInfo directory = new DirectoryInfo(sourceDirectory);
            if (!directory.Exists) throw new DirectoryNotFoundException($"{nameof(sourceDirectory)} '{directory.FullName}' が見つかりませんでした。");

            DirectoryInfo[] directories = directory.GetDirectories();
            Directory.CreateDirectory(destDirectory);

            foreach (FileInfo file in directory.GetFiles())
            {
                string targetFilePath = Path.Combine(destDirectory, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDirectory in directories)
                {
                    string newDestDirectory = Path.Combine(destDirectory, subDirectory.Name);
                    CopyDirectory(subDirectory.FullName, newDestDirectory, true);
                }
            }
        }
    }
}
