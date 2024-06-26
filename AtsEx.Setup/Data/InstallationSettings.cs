using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AtsEx.Setup.Data
{
    [XmlRoot]
    public class InstallationSettings
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(InstallationSettings));
        private static readonly string FileName = nameof(InstallationSettings) + ".xml";
        private static readonly string FilePath = Path.Combine(ApplicationInfo.AtsExDirectory, FileName);

        internal static InstallationSettings Loaded { get; }

        static InstallationSettings()
        {
            Loaded = Load();


            InstallationSettings Load()
            {
                if (!File.Exists(FilePath)) return new InstallationSettings();

                try
                {
                    using (StreamReader sr = new StreamReader(FilePath, Encoding.UTF8))
                    {
                        InstallationSettings result = (InstallationSettings)Serializer.Deserialize(sr);
                        return result;
                    }
                }
                catch
                {
                    return new InstallationSettings();
                }
            }
        }


        public string Bve6Path = null;
        public string Bve5Path = null;
        public string ScenarioDirectory = null;

        internal void Save()
        {
            using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8))
            {
                Serializer.Serialize(sw, this);
            }
        }
    }
}
