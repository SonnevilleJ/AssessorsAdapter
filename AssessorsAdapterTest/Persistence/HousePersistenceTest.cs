using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class HousePersistenceTest
    {
        private static readonly IHouse TestHouse = HouseTest.TestHouse;
        
        [TestMethod]
        public void SaveHouse()
        {
            var repo = new MockRepository<string, IHouse>();
            var house = TestHouse;
            repo.Save(house.Address, house);

            Assert.IsTrue(repo.Contains(house.Address, house));
        }

        [TestMethod]
        public void GetHouse()
        {
            var repo = new MockRepository<string, IHouse>();
            var house = TestHouse;
            repo.Save(house.Address, house);

            var result = repo.Get(house.Address);

            Assert.AreEqual(house, result);
        }
    }
}