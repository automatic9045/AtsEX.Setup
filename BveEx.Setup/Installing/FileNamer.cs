using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup.Installing
{
    internal static class FileNamer
    {
        public static string CreateFilePathInSequence(string pathBase)
        {
            string extension = Path.GetExtension(pathBase);
            string pathBaseWithoutExtension = Path.Combine(Path.GetDirectoryName(pathBase), Path.GetFileNameWithoutExtension(pathBase));

            string pathWithoutExtension = pathBaseWithoutExtension;
            int i = 0;
            while (File.Exists(pathWithoutExtension + extension))
            {
                i++;
                pathWithoutExtension = $"{pathBaseWithoutExtension} ({i})";
            }

            return pathWithoutExtension + extension;
        }

        public static string CreateDirectoryNameInSequence(string pathBase)
        {
            string path = pathBase;
            int i = 0;
            while (Directory.Exists(path))
            {
                i++;
                path = $"{pathBase} ({i})";
            }

            return path;
        }
    }
}
