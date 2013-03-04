using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseTest : HouseTestBase
    {
        private static readonly House TestHouse = ConstructHouse(Address);

        private const string Address = "6324 Wilcot Ct";
        private const string City = "Johnston";
        private const int Zip = 50131;
        private const int Assessment = 291600;
        private const int Land = 13740;
        private const int Tsfla = 2266;

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
        public void TsflaMatches()
        {
            VerifyTsflaMatches(Tsfla, TestHouse);
        }
    }
}
