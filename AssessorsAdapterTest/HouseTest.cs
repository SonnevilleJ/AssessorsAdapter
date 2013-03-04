using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseTest
    {
        private const string Address = "6324 Wilcot Ct";
        private const string City = "Johnston";
        private const int ExpectedAssessment = 291600;

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
            VerifyAddressMatches(Address, house);
        }

        [TestMethod]
        public void CityMatches()
        {
            var house = new House(Address);
            VerifyCityMatches(City, house);
        }

        [TestMethod]
        public void ZipMatches()
        {
            var house = new House(Address);
            VerifyZipMatches(50131, house);
        }

        private static void VerifyAddressMatches(string address, House house)
        {
            Assert.AreEqual(address, house.Address, true);
        }

        private void VerifyCityMatches(string city, House house)
        {
            Assert.AreEqual(city, house.City, true);
        }

        private void VerifyZipMatches(int zip, House house)
        {
            VerifyZipMatches(zip.ToString(), house);
        }

        private static void VerifyZipMatches(string zip, House house)
        {
            Assert.AreEqual(zip, house.Zip);
        }

        [TestMethod]
        public void AndysAddressMatches()
        {
            var house = new House(AndysAddress);
            VerifyAddressMatches(AndysAddress, house);
        }

        [TestMethod]
        public void AndysCityMatches()
        {
            var house = new House(AndysAddress);
            VerifyCityMatches("Polk City", house);
        }

        [TestMethod]
        public void AndysZipMatches()
        {
            var house = new House(AndysAddress);
            VerifyZipMatches(50226, house);
        }
    }
}
