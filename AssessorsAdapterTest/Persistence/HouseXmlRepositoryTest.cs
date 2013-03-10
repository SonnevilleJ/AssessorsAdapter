using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssessorsAdapter;
using AssessorsAdapter.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class HouseXmlRepositoryTest
    {
        private static readonly IHouse TestHouse = HouseTest.TestHouse;

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
                var house = HouseFactory.Clone(TestHouse);

                repo.Save(house.Address, house);

                Assert.IsTrue(HouseFoundInPath(path, house));
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

            repo.Save(TestHouse.Address, TestHouse);

            Assert.IsTrue(repo.ContainsValue(TestHouse));
        }

        [TestMethod]
        public void ContainsKey()
        {
            var repo = GetTestRepo();

            repo.Save(TestHouse.Address, TestHouse);

            Assert.IsTrue(repo.ContainsKey(TestHouse.Address));
        }

        [TestMethod]
        public void DeleteExisting()
        {
            var repo = GetTestRepo();

            repo.Save(TestHouse.Address, TestHouse);

            repo.Delete(TestHouse.Address);

            Assert.IsFalse(HouseFoundInPath(repo.StoragePath, HouseFactory.Clone(TestHouse)));
        }

        [TestMethod]
        public void FetchReturnsSame()
        {
            var repo = GetTestRepo();

            var house = TestHouse;
            var address = house.Address;
            repo.Save(address, house);

            var value = repo.Fetch(address);
            Assert.IsTrue(house.Equals(value));
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

                repo.Save(TestHouse.Address, TestHouse);

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

        private static bool HouseFoundInPath(string path, IEquatable<IHouse> house)
        {
            var houseFound = false;
            foreach (var filename in FilesInPath(path).Where(file =>
                {
                    var extension = Path.GetExtension(file);
                    return extension != null && extension.ToLower() == ".xml";
                }))
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
    }
}
