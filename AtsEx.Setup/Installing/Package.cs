using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.Installing
{
    internal class Package
    {
        private readonly Stream Source;

        public Package(Stream source)
        {
            Source = source;
        }

        public static Package FromResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);

            return new Package(stream);
        }

        public void Locate(string destPath)
        {
            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }

            using (FileStream fileStream = new FileStream(destPath, FileMode.CreateNew, FileAccess.Write))
            {
                Source.CopyTo(fileStream, (int)Source.Length);
                Source.Position -= Source.Length;
            }
        }
    }
}
