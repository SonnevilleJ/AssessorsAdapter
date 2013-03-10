using AssessorsAdapter;
using AssessorsAdapter.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseTest : HouseTestBase
    {
        public static readonly IHouse TestHouse = HouseFactory.ConstructHouse(Address);

        private const string Address = "6324 Wilcot Ct";
        private const string City = "Johnston";
        private const int Zip = 50131;
        private const int Assessment = 291600;
        private const int Land = 13740;
        private const int Tsfla = 2266;
        private const int BsmtArea = 1141;
        private const int YearBuilt = 2004;
        private const int Fireplaces = 1;
        private const decimal Taxes = 5875.00m;

        [TestMethod]
        public void HouseConstructor()
        {
            Assert.IsNotNull(TestHouse);
        }

        [TestMethod]
        public void DataAvailable()
        {
            Assert.IsTrue(TestHouse.DataAvailable);
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

        [TestMethod]
        public void BasementMatches()
        {
            VerifyBasementMatches(BsmtArea, TestHouse);
        }

        [TestMethod]
        public void YearBuiltMatches()
        {
            VerifyYearBuiltMatches(YearBuilt, TestHouse);
        }

        [TestMethod]
        public void FireplacesMatches()
        {
            VerifyFireplacesMatches(Fireplaces, TestHouse);
        }

        [TestMethod]
        public void TaxesMatch()
        {
            VerifyTaxesMatch(Taxes, TestHouse);
        }

        [TestMethod]
        public void ConvertionEqualsOriginal()
        {
            var house = HouseFactory.Clone(TestHouse);

            Assert.AreEqual(TestHouse, house);
        }

        [TestMethod]
        public void SerializationTest()
        {
            var testHouse = TestHouse;
            var xml = XmlSerializer.SerializeToXml(testHouse);
            var deserialized = XmlSerializer.DeserializeFromXml<IHouse>(xml);

            Assert.AreEqual(testHouse, deserialized);
        }
    }
}
