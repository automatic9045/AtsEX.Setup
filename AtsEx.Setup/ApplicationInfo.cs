using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace AtsEx.Setup
{
    internal static class ApplicationInfo
    {
        private static readonly string CommonDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        public static readonly string AtsExDirectory = Path.Combine(CommonDocumentsDirectory, "AtsEx");
    }
}
