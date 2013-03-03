using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string Address = "6324 Wilcot Ct";
        private const string City = "Johnston";

        private const string AndysAddress = "9260 NW 36th St";

        [TestMethod]
        public void HouseConstructor()
        {
            var house = new House(Address, City);
            Assert.IsNotNull(house);
        }

        [TestMethod]
        public void AddressMatches()
        {
            var house = new House(Address);
            Assert.AreEqual(Address, house.Address);
        }

        [TestMethod]
        public void AndysAddressMatches()
        {
            var house = new House(AndysAddress);
            Assert.AreEqual(AndysAddress, house.Address);
        }
    }
}
