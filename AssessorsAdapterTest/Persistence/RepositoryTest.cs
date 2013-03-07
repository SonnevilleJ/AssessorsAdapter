using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void SaveTest()
        {
            var repo = new MockRepository<int, string>();
            const int key = 5;
            const string value = "five";
            repo.Save(key, value);

            Assert.IsTrue(repo.Contains(key, value));
        }

        [TestMethod]
        public void GetTest()
        {
            var repo = new MockRepository<int, string>();
            const int key = 5;
            const string value = "five";
            repo.Save(key, value);

            var result = repo.Get(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetMissingItemTest()
        {
            var repo = new MockRepository<int, string>();
            const int key = 5;
            const string value = "five";
            repo.Save(key, value);

            repo.Get(2);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var repo = new MockRepository<int, string>();
            const int key = 5;
            const string value = "five";
            repo.Save(key, value);
            repo.Delete(key);

            Assert.IsFalse(repo.Contains(key, value));
        }
    }
}
