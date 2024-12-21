using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BveEx.Setup.Installing
{
    internal class PreferenceEditor
    {
        private readonly string XmlPath;
        private readonly XDocument Document;
        private readonly XElement Root;

        public PreferenceEditor(string xmlPath)
        {
            XmlPath = xmlPath;
            Document = XDocument.Load(XmlPath);
            Root = Document.Element("Preferences");
        }

        public static PreferenceEditor TryCreateFromUser(WinUser user, string xmlFileName)
        {
            string path = Path.Combine(user.DocumentDirectory, @"BveTs\Settings", xmlFileName);
            return File.Exists(path) ? new PreferenceEditor(path) : null;
        }

        public void AddInputDevice(string deviceName)
        {
            XElement pluginsElement = Root.Element("InputPlugins");
            if (pluginsElement is null)
            {
                pluginsElement = new XElement("InputPlugins");
                Root.Add(pluginsElement);
            }

            XElement content = new XElement("string", deviceName);
            if (!pluginsElement.Elements().Contains(content, new ElementEqualityComparer())) pluginsElement.Add(content);
        }

        public void Save()
        {
            Document.Save(XmlPath);
        }


        private class ElementEqualityComparer : IEqualityComparer<XElement>
        {
            public bool Equals(XElement x, XElement y) => x.Name == y.Name && x.Value == y.Value;
            public int GetHashCode(XElement obj) => (obj.Name, obj.Value).GetHashCode();
        }
    }
}
