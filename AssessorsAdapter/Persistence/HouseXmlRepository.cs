using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssessorsAdapter.Persistence
{
    public class HouseXmlRepository : IRepository<string, IHouse>
    {
        public HouseXmlRepository(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        public void Save(string key, IHouse value)
        {
            if (value.GetType().Name != typeof (PersistedHouse).Name)
                value = PersistedHouse.FromIHouse(value);

            var address = key;
            var serialized = XmlSerializer.SerializeToXml(value);

            var fullPath = FormatFilename(address);
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
                return XmlSerializer.DeserializeFromXml<PersistedHouse>(streamReader.ReadToEnd());
            }
        }

        private string FormatFilename(string address)
        {
            return string.Format("{0}{1}{2}", Path, address, ".xml");
        }

        private IEnumerable<string> StoredKeys
        {
            get
            {
                var files = Directory.EnumerateFiles(Path, "*.xml", SearchOption.TopDirectoryOnly);
                return (files.Select(file => new FileInfo(file)).Select(fi => fi.Name).Select(fileName => fileName.Replace(".xml", string.Empty))).ToList();
            }
        }
    }
}