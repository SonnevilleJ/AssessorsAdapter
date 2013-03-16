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
        private static HtmlDocument _housePage;
        private static HtmlDocument _taxPage;
        private readonly HouseFactory _factory = new HouseFactory();
        private IHouse _testHouse;
        private string _path;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _housePage = new HtmlDocument();
            _housePage.LoadHtml(Resources._6324_Wilcot_Ct);
            _taxPage = new HtmlDocument();
            _taxPage.LoadHtml(Resources._6324_Wilcot_Ct___taxes);
        }

        [TestInitialize]
        public void Initialize()
        {
            _path = GetUniqueTempPath();
            _testHouse = _factory.ConstructHouse(_housePage, _taxPage);
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

            Assert.IsFalse(repo.ContainsValue(house));
        }

        [TestMethod]
        public void SaveSerializesInPath()
        {
                var repo = GetTestRepo();
                var house = _factory.Clone(_testHouse);

                repo.Save(house.Address, house);

                Assert.IsTrue(HouseIsFoundInPath(_path, house));
        }

        [TestMethod]
        public void ContainsValue()
        {
            var repo = GetTestRepo();

            repo.Save(_testHouse.Address, _testHouse);

            Assert.IsTrue(repo.ContainsValue(_testHouse));
        }

        [TestMethod]
        public void ContainsKey()
        {
            var repo = GetTestRepo();

            repo.Save(_testHouse.Address, _testHouse);

            Assert.IsTrue(repo.ContainsKey(_testHouse.Address));
        }

        [TestMethod]
        public void DeleteExisting()
        {
            var repo = GetTestRepo();
            repo.Save(_testHouse.Address, _testHouse);

            repo.Delete(_testHouse.Address);

            Assert.IsFalse(HouseIsFoundInPath(repo.StoragePath, _factory.Clone(_testHouse)));
        }

        [TestMethod]
        public void FetchReturnsSame()
        {
            var repo = GetTestRepo();
            repo.Save(_testHouse.Address, _testHouse);

            var value = repo.Fetch(_testHouse.Address);
            
            Assert.AreEqual(_testHouse, value);
        }

        [TestMethod]
        public void EmptyClearsSerializedObjects()
        {
                var repo = GetTestRepo();

                if (new DirectoryInfo(_path).GetFiles().Length != 0) Assert.Inconclusive();

                repo.Save(_testHouse.Address, _testHouse);

                var houses = HousesFoundInPath(repo.StoragePath);
                if (houses.Count() != 1) Assert.Inconclusive();

                repo.Empty();

                houses = HousesFoundInPath(repo.StoragePath);
                Assert.AreEqual(0, houses.Count());
        }

        #region Private Methods

        private static string GetUniqueTempPath()
        {
            return String.Format("{0}{1}", Path.GetTempPath(), Guid.NewGuid());
        }

        private XmlRepository<IHouse> GetTestRepo()
        {
            return new XmlRepository<IHouse>(_path);
        }

        private static IEnumerable<string> FilesInPath(string path)
        {
            return Directory.GetFiles(path, "*.xml");
        }

        private static IList<House> HousesFoundInPath(string path)
        {
            var files = FilesInPath(path);
            var list = new List<House>();
            foreach (var file in files)
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

        private static bool HouseIsFoundInPath(string path, IEquatable<IHouse> house)
        {
            var houseFound = false;
            foreach (var filename in GetXmlFiles(path))
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

        private static IEnumerable<string> GetXmlFiles(string path)
        {
            return FilesInPath(path).Where(file =>
                {
                    var extension = Path.GetExtension(file);
                    return extension != null && extension.ToLower() == ".xml";
                });
        }

        #endregion
    }
}
