using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssessorsAdapter.Persistence
{
    public class XmlRepository<T> : IRepository<string, T>
    {
        private const string IndexFile = "Index";

        public XmlRepository(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            StoragePath = path;
        }

        public void Save(string key, T value)
        {
            PersistValue(key, value);

            PersistIndex();
        }

        public void Delete(string key)
        {
            var filename = FormatValueFilename(key);
            if (File.Exists(filename)) File.Delete(filename);
        }

        public bool ContainsValue(T value)
        {
            return StoredKeys.Select(Fetch).Contains(value);
        }

        public bool ContainsKey(string key)
        {
            return StoredKeys.Contains(key);
        }

        public T Fetch(string key)
        {
            return DepersistValue(key);
        }

        public void Empty()
        {
            foreach (var key in StoredKeys)
            {
                Delete(key);
            }
            File.Delete(FormatIndexFilename());
        }

        public string StoragePath { get; private set; }

        private IEnumerable<string> StoredKeys
        {
            get
            {
                var values = Directory.GetFiles(StoragePath, "*.xml", SearchOption.TopDirectoryOnly).Where(filename => filename != FormatIndexFilename());
                return (values.Select(file => new FileInfo(file)).Select(fi => fi.Name).Select(fileName => fileName.Replace(".xml", string.Empty))).ToList();
            }
        }

        private void PersistValue(string key, T value)
        {
            var fullPath = FormatValueFilename(key);
            File.WriteAllText(fullPath, XmlSerializer.SerializeToXml(value));
        }

        private T DepersistValue(string key)
        {
            var fullPath = FormatValueFilename(key);
            return XmlSerializer.DeserializeFromXml<T>(File.ReadAllText(fullPath));
        }

        private void PersistIndex()
        {
            File.WriteAllLines(FormatIndexFilename(), StoredKeys);
        }

        private string FormatIndexFilename()
        {
            return string.Format("{0}{1}{2}{3}", StoragePath, Path.DirectorySeparatorChar, IndexFile, ".xml");
        }

        private string FormatValueFilename(string value)
        {
            return string.Format("{0}{1}{2}{3}", StoragePath, Path.DirectorySeparatorChar, value, ".xml");
        }
    }
}