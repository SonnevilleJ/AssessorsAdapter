using System.Globalization;
using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseTest
    {
        private static readonly House TestHouse = ConstructHouse(Address);

        private static readonly House AndysHouse = ConstructHouse(AndysAddress);

        private static readonly House DuplicateAddresses = ConstructHouse(MultipleAddresses);

        private static House ConstructHouse(string address)
        {
            var house = new House();
            house.FetchData(address);
            return house;
        }

        private const string Address = "6324 Wilcot Ct";
        private const string City = "Johnston";
        private const int Zip = 50131;
        private const int Assessment = 291600;
        private const int Land = 13740;
        private const int Tsfla = 2266;

        private const string AndysAddress = "9260 NW 36th St";
        private const string MultipleAddresses = "9823 Laguna Dr";

        [TestMethod]
        public void HouseConstructor()
        {
            Assert.IsNotNull(TestHouse);
        }

        [TestMethod]
        public void AddressMatches()
        {
            VerifyAddressMatches(Address, TestHouse);
        }

        [TestMethod]
        public void CityMatches()
        {
            VerifyCityMatches(City, TestHouse);
        }

        [TestMethod]
        public void ZipMatches()
        {
            VerifyZipMatches(Zip, TestHouse);
        }

        [TestMethod]
        public void AssessmentMatches()
        {
            VerifyAssessmentMatches(Assessment, TestHouse);
        }

        [TestMethod]
        public void LandMatches()
        {
            VerifyLandMatches(Land, TestHouse);
        }

        [TestMethod]
        public void NoResultsTest()
        {
            var house = ConstructHouse("123 Fake St");
            Assert.IsTrue(house.NoRecordsFound);
        }

        [TestMethod]
        public void MultipleResultsTest()
        {
            Assert.IsTrue(DuplicateAddresses.MultipleRecordsFound);
        }

        [TestMethod]
        public void TsflaMatches()
        {
            Assert.AreEqual(Tsfla, TestHouse.TSFLA);
        }

        #region Verify methods

        private static void VerifyAddressMatches(string address, House house)
        {
            Assert.AreEqual(address, house.Address, true);
        }

        private static void VerifyCityMatches(string city, House house)
        {
            Assert.AreEqual(city, house.City, true);
        }

        private static void VerifyZipMatches(int zip, House house)
        {
            VerifyZipMatches(zip.ToString(CultureInfo.InvariantCulture), house);
        }

        private static void VerifyZipMatches(string zip, House house)
        {
            Assert.AreEqual(zip, house.Zip);
        }

        private static void VerifyAssessmentMatches(int assessment, House house)
        {
            Assert.AreEqual(assessment, house.AssessmentTotal);
        }

        private static void VerifyLandMatches(int land, House house)
        {
            Assert.AreEqual(land, house.Land);
        }

        #endregion

        [TestMethod]
        public void AndysAddressMatches()
        {
            VerifyAddressMatches(AndysAddress, AndysHouse);
        }

        [TestMethod]
        public void AndysCityMatches()
        {
            VerifyCityMatches("Polk City", AndysHouse);
        }

        [TestMethod]
        public void AndysZipMatches()
        {
            VerifyZipMatches(50226, AndysHouse);
        }
    }
}
