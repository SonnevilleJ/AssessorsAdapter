using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssessorsAdapter.Persistence
{
    public class HouseXmlRepository : IRepository<string, IHouse>
    {
        public HouseXmlRepository(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            StoragePath = path;
        }

        public void Save(string key, IHouse value)
        {
            var serialized = XmlSerializer.SerializeToXml(value);

            var fullPath = FormatFilename(key);
            File.WriteAllText(fullPath, serialized);
        }

        public void Delete(string key)
        {
            var filename = FormatFilename(key);
            if (File.Exists(filename)) File.Delete(filename);
        }

        public bool ContainsValue(IHouse value)
        {
            return StoredKeys.Contains(value.Address);
        }

        public bool ContainsKey(string key)
        {
            return StoredKeys.Contains(key);
        }

        public IHouse Fetch(string key)
        {
            using (var streamReader = new StreamReader(FormatFilename(key)))
            {
                return XmlSerializer.DeserializeFromXml<House>(streamReader.ReadToEnd());
            }
        }

        public void Empty()
        {
            foreach (var key in StoredKeys)
            {
                Delete(key);
            }
        }

        public string StoragePath { get; private set; }

        private string FormatFilename(string address)
        {
            return string.Format("{0}{1}{2}{3}", StoragePath, Path.DirectorySeparatorChar, address, ".xml");
        }

        private IEnumerable<string> StoredKeys
        {
            get
            {
                var files = Directory.EnumerateFiles(StoragePath, "*.xml", SearchOption.TopDirectoryOnly);
                return (files.Select(file => new FileInfo(file)).Select(fi => fi.Name).Select(fileName => fileName.Replace(".xml", string.Empty))).ToList();
            }
        }
    }
}