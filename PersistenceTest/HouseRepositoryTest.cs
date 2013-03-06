using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PersistenceTest
{
    [TestClass]
    public class HouseRepositoryTest
    {
        private static readonly IHouse TestHouse = ConstructHouse(Address);

        private const string Address = "6324 Wilcot Ct";

        private static IHouse ConstructHouse(string address)
        {
            var house = new AssessorsHouse();
            house.FetchData(address);
            return house;
        }
        
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