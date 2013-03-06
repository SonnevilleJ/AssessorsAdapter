using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PersistenceTest
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void SaveTest()
        {
            var repo = new MockRepository<int>();
            const int obj = 5;
            repo.Save(obj);

            Assert.IsTrue(repo.Contains(obj));
        }

        [TestMethod]
        public void DeleteTest()
        {
            var repo = new MockRepository<int>();
            const int obj = 5;
            repo.Save(obj);
            repo.Delete(obj);

            Assert.IsFalse(repo.Contains(obj));
        }
    }
}
