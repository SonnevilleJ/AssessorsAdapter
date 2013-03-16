using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssessorsAdapter;
using AssessorsAdapter.Persistence;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class HouseXmlRepositoryTest
    {
        private const string IndexFilename = "Index";
        private readonly HouseFactory _factory = new HouseFactory();
        private string _path;

        [TestInitialize]
        public void Initialize()
        {
            _path = GetUniqueTempPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(_path, true);
        }

        [TestMethod]
        public void PathTest()
        {
            var repo = GetTestRepo();

            Assert.AreEqual(_path, repo.StoragePath);
        }

        [TestMethod]
        public void PathCreatesDirectoryTest()
        {
            var repo = GetTestRepo();

            Assert.IsTrue(Directory.Exists(repo.StoragePath));
        }

        [TestMethod]
        public void ContainsWhenEmpty()
        {
            var repo = GetTestRepo();
            var house = new House();

            Assert.IsFalse(repo.ContainsKey(house.Address));
        }

        [TestMethod]
        public void SaveSerializesInPath()
        {
            var repo = GetTestRepo();
            var house = _factory.Clone(House1);

            repo.Save(house.Address, house);

            Assert.IsTrue(HouseIsFound(house));
        }

        [TestMethod]
        public void SaveSerializesUpdatedIndex()
        {
            var repo = GetTestRepo();
            var house = House1;

            var key = house.Address;
            repo.Save(key, house);

            Assert.IsTrue(IndexContainsKey(key));
        }

        [TestMethod]
        public void ContainsValue()
        {
            var repo = GetTestRepo();

            repo.Save(House1.Address, House1);

            Assert.IsTrue(repo.ContainsValue(House1));
        }

        [TestMethod]
        public void ContainsKey()
        {
            var repo = GetTestRepo();

            repo.Save(House1.Address, House1);

            Assert.IsTrue(repo.ContainsKey(House1.Address));
        }

        [TestMethod]
        public void DeleteExisting()
        {
            var repo = GetTestRepo();
            repo.Save(House1.Address, House1);

            repo.Delete(House1.Address);

            Assert.IsFalse(HouseIsFound(_factory.Clone(House1)));
        }

        [TestMethod]
        public void FetchReturnsSame()
        {
            var repo = GetTestRepo();
            repo.Save(House1.Address, House1);

            var value = repo.Fetch(House1.Address);

            Assert.AreEqual(House1, value);
        }

        [TestMethod]
        public void EmptyClearsSerializedObjects()
        {
            var repo = GetTestRepo();
            repo.Save(House1.Address, House1);

            repo.Empty();

            Assert.AreEqual(0, HousesFound().Count());
        }

        [TestMethod]
        public void EmptyClearsSerializedIndex()
        {
            var repo = GetTestRepo();
            repo.Save(House1.Address, House1);

            repo.Empty();

            Assert.IsFalse(File.Exists(GetIndexFilePath()));
        }

        #region Private Methods

        private IHouse House1
        {
            get { return GetHouse(Resources._6324_Wilcot_Ct, Resources._6324_Wilcot_Ct___taxes); }
        }

        private IHouse House2
        {
            get { return GetHouse(Resources._9260_NW_36th_St, Resources._9260_NW_36th_St___taxes); }
        }

        private IHouse GetHouse(string houseHtml, string taxesHtml)
        {
            var houseDoc = new HtmlDocument();
            houseDoc.LoadHtml(houseHtml);
            var taxesDoc = new HtmlDocument();
            taxesDoc.LoadHtml(taxesHtml);
            return _factory.ConstructHouse(houseDoc, taxesDoc);
        }

        private static string GetUniqueTempPath()
        {
            return String.Format("{0}{1}", Path.GetTempPath(), Guid.NewGuid());
        }

        private XmlRepository<IHouse> GetTestRepo()
        {
            return new XmlRepository<IHouse>(_path);
        }

        private bool IndexContainsKey(string key)
        {
            var index = File.ReadAllLines(GetIndexFilePath());
            return index.Contains(key);
        }

        private string GetIndexFilePath()
        {
            return string.Format("{0}{1}{2}{3}", _path, Path.DirectorySeparatorChar, IndexFilename, ".xml");
        }

        private IEnumerable<string> FilesFound()
        {
            return Directory.GetFiles(_path, "*.xml");
        }

        private IEnumerable<string> GetXmlFiles()
        {
            var xmlFiles = FilesFound().Where(file =>
                {
                    var extension = Path.GetExtension(file);
                    return extension != null && extension.ToLower() == ".xml";
                });
            return xmlFiles;
        }

        private IList<House> HousesFound()
        {
            var files = FilesFound();
            var list = new List<House>();
            foreach (var file in files.Where(f => f != IndexFilename))
            {
                string contents;
                using (var reader = new StreamReader(file))
                {
                    contents = reader.ReadToEnd();
                }
                var deserialized = XmlSerializer.DeserializeFromXml<House>(contents);
                list.Add(deserialized);
            }
            return list;
        }

        private bool HouseIsFound(IHouse house)
        {
            var houseFound = false;
            foreach (var filename in GetXmlFiles())
            {
                using (var streamReader = new StreamReader(filename))
                {
                    var contents = streamReader.ReadToEnd();
                    try
                    {
                        var deserialized = XmlSerializer.DeserializeFromXml<IHouse>(contents);
                        if (house.Equals(deserialized))
                        {
                            houseFound = true;
                            break;
                        }
                    }
                    catch (TypeLoadException)
                    {
                    }
                }
            }
            return houseFound;
        }

        #endregion
    }
}
