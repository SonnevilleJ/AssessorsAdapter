using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssessorsAdapter;

namespace Persistence
{
    public class XmlRepository : IRepository<string, IHouse>
    {
        private readonly string _path;

        public XmlRepository(string path)
        {
            _path = path;
        }

        public void Save(string key, IHouse value)
        {
            if (value.GetType().Name != typeof (PersistedHouse).Name)
                value = PersistedHouse.FromIHouse(value);

            var address = key;
            var serialized = XmlSerializer.SerializeToXml(value);

            var fullPath = FormatFilename(address);
            File.WriteAllText(fullPath, serialized);
        }

        private string FormatFilename(string address)
        {
            return string.Format("{0}{1}{2}", _path, address, ".xml");
        }

        public void Delete(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsValue(IHouse value)
        {
            foreach (var filename in StoredValues)
            {
                using (var streamReader = new StreamReader(filename))
                {
                    var contents = streamReader.ReadToEnd();
                    try
                    {
                        var deserialized = XmlSerializer.DeserializeFromXml<PersistedHouse>(contents);
                        if (deserialized.Equals(value)) return true;
                    }
                    catch (TypeLoadException)
                    {
                    }
                }
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            var filename = FormatFilename(key);
            return StoredValues.Contains(filename);
        }

        private IEnumerable<string> StoredValues
        {
            get { return Directory.GetFiles(_path, "*.xml", SearchOption.TopDirectoryOnly); }
        }
    }
}