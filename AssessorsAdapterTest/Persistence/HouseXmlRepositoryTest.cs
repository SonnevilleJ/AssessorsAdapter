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
        private readonly HouseFactory _factory = new HouseFactory();
        private IHouse _testHouse;

        [TestInitialize]
        public void Initialize()
        {
            var housePage = new HtmlDocument();
            housePage.LoadHtml(Resources._6324_Wilcot_Ct);
            var taxPage = new HtmlDocument();
            taxPage.LoadHtml(Resources._6324_Wilcot_Ct___taxes);
            _testHouse = _factory.ConstructHouse(housePage, taxPage);
        }

        [TestMethod]
        public void PathTest()
        {
            var path = Path.GetTempPath();
            var repo = new HouseXmlRepository(path);

            Assert.AreEqual(path, repo.StoragePath);
        }

        [TestMethod]
        public void PathCreatesDirectoryTest()
        {
            var path = GetUniqueTempPath();
            try
            {
                if (Directory.Exists(path)) Assert.Inconclusive();

                var repo = new HouseXmlRepository(path);

                Assert.IsTrue(Directory.Exists(repo.StoragePath));
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        [TestMethod]
        public void ContainsWhenEmpty()
        {
            var path = Path.GetTempPath();
            var repo = new HouseXmlRepository(path);
            var house = new House();

            Assert.IsFalse(repo.ContainsValue(house));
        }

        [TestMethod]
        public void SaveSerializesInPath()
        {
            var path = GetUniqueTempPath();
            try
            {
                var repo = GetTestRepo(path);
                var house = _factory.Clone(_testHouse);

                repo.Save(house.Address, house);

                Assert.IsTrue(HouseIsFoundInPath(path, house));
            }
            finally
            {
                Directory.Delete(path, true);
            }
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

            var house = _testHouse;
            var address = house.Address;
            repo.Save(address, house);

            var value = repo.Fetch(address);
            Assert.AreEqual(house, value);
        }

        [TestMethod]
        public void EmptyClearsSerializedObjects()
        {
            var path = GetUniqueTempPath();
            try
            {
                var repo = GetTestRepo(path);

                if (new DirectoryInfo(path).GetFiles().Length != 0) Assert.Inconclusive();

                repo.Save(_testHouse.Address, _testHouse);

                var houses = HousesFoundInPath(repo.StoragePath);
                if (houses.Count() != 1) Assert.Inconclusive();

                repo.Empty();

                houses = HousesFoundInPath(repo.StoragePath);
                Assert.AreEqual(0, houses.Count());
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        #region Private Methods

        private static string GetUniqueTempPath()
        {
            return String.Format("{0}{1}", Path.GetTempPath(), Guid.NewGuid());
        }

        private static HouseXmlRepository GetTestRepo()
        {
            return GetTestRepo(GetUniqueTempPath());
        }

        private static HouseXmlRepository GetTestRepo(string path)
        {
            return new HouseXmlRepository(path);
        }

        private static IEnumerable<string> FilesInPath(string path)
        {
            return Directory.GetFiles(path, "*.xml");
        }

        private static List<House> HousesFoundInPath(string path)
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
