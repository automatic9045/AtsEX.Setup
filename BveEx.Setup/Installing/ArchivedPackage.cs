using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup.Installing
{
    internal class ArchivedPackage
    {
        private readonly ZipArchive Archive;

        public ArchivedPackage(Stream source)
        {
            Archive = new ZipArchive(source);
        }

        public static ArchivedPackage FromResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);

            return new ArchivedPackage(stream);
        }

        public void ExtractAndLocate(string destPath)
        {
            destPath = Path.GetFullPath(destPath);

            foreach (ZipArchiveEntry entry in Archive.Entries)
            {
                string path = Path.GetFullPath(Path.Combine(destPath, entry.FullName));
                if (!path.StartsWith(destPath)) continue;

                if (entry.Name == "")
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    entry.ExtractToFile(path, true);
                }
            }
        }
    }
}
