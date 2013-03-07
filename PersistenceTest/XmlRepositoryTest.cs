using System;
using System.IO;
using System.Linq;
using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceTest
{
    [TestClass]
    public class XmlRepositoryTest
    {
        private static readonly AssessorsHouse TestHouse = ConstructHouse(Address);

        private const string Address = "6324 Wilcot Ct";

        private static AssessorsHouse ConstructHouse(string address)
        {
            var house = new AssessorsHouse();
            house.FetchData(address);
            return house;
        }

        [TestMethod]
        public void ContainsWhenEmpty()
        {
            var path = Path.GetTempPath();
            var repo = new XmlRepository(path);
            var house = new PersistedHouse();

            Assert.IsFalse(repo.ContainsValue(house));
        }

        [TestMethod]
        public void SaveSerializesInPath()
        {
            var path = Path.GetTempPath();
            var repo = GetTestRepo(path);
            var house = PersistedHouse.FromIHouse(TestHouse);

            repo.Save(house.Address, house);

            var houseFound = false;
            foreach (var filename in Directory.EnumerateFiles(path)
                                              .Where(file =>
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
            Assert.IsTrue(houseFound);
        }

        [TestMethod]
        public void SaveIHouseConvertsToPersistedHouse()
        {
            var persistedHouse = PersistedHouse.FromIHouse(TestHouse);
            var path = Path.GetTempPath();
            var repo = GetTestRepo();

            repo.Save(TestHouse.Address, TestHouse);

            var houseFound = false;
            foreach (var filename in Directory.EnumerateFiles(path)
                                  .Where(file =>
                                  {
                                      var extension = Path.GetExtension(file);
                                      return extension != null && extension.ToLower() == ".xml";
                                  }))
            {
                using (var streamReader = new StreamReader(filename))
                {
                    var contents = streamReader.ReadToEnd();
                    var typeName = persistedHouse.GetType().Name;
                    if (contents.Contains(typeName))
                    {
                        houseFound = true;
                    }
                }
            }
            Assert.IsTrue(houseFound);
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

        private static XmlRepository GetTestRepo()
        {
            return GetTestRepo(Path.GetTempPath());
        }

        private static XmlRepository GetTestRepo(string path)
        {
            return new XmlRepository(path);
        }
    }
}
