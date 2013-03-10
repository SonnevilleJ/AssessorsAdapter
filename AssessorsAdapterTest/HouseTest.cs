using AssessorsAdapter;
using AssessorsAdapter.Persistence;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseTest : HouseTestBase
    {
        private IHouse _testHouse;
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

        [TestInitialize]
        public void Initialize()
        {
            var housePage = new HtmlDocument();
            housePage.LoadHtml(Resources._6324_Wilcot_Ct);
            var taxPage = new HtmlDocument();
            taxPage.LoadHtml(Resources._6324_Wilcot_Ct___taxes);
            _testHouse = HouseFactory.ConstructHouse(housePage, taxPage);
        }

        [TestMethod]
        public void HouseConstructor()
        {
            Assert.IsNotNull(_testHouse);
        }

        [TestMethod]
        public void DataAvailable()
        {
            Assert.IsTrue(_testHouse.DataAvailable);
        }

        [TestMethod]
        public void AddressMatches()
        {
            VerifyAddressMatches(Address, _testHouse);
        }

        [TestMethod]
        public void CityMatches()
        {
            VerifyCityMatches(City, _testHouse);
        }

        [TestMethod]
        public void ZipMatches()
        {
            VerifyZipMatches(Zip, _testHouse);
        }

        [TestMethod]
        public void AssessmentMatches()
        {
            VerifyAssessmentMatches(Assessment, _testHouse);
        }

        [TestMethod]
        public void LandMatches()
        {
            VerifyLandMatches(Land, _testHouse);
        }

        [TestMethod]
        public void TsflaMatches()
        {
            VerifyTsflaMatches(Tsfla, _testHouse);
        }

        [TestMethod]
        public void BasementMatches()
        {
            VerifyBasementMatches(BsmtArea, _testHouse);
        }

        [TestMethod]
        public void YearBuiltMatches()
        {
            VerifyYearBuiltMatches(YearBuilt, _testHouse);
        }

        [TestMethod]
        public void FireplacesMatches()
        {
            VerifyFireplacesMatches(Fireplaces, _testHouse);
        }

        [TestMethod]
        public void TaxesMatch()
        {
            VerifyTaxesMatch(Taxes, _testHouse);
        }

        [TestMethod]
        public void ConvertionEqualsOriginal()
        {
            var house = HouseFactory.Clone(_testHouse);

            Assert.AreEqual(_testHouse, house);
        }

        [TestMethod]
        public void SerializationTest()
        {
            var testHouse = _testHouse;
            var xml = XmlSerializer.SerializeToXml(testHouse);
            var deserialized = XmlSerializer.DeserializeFromXml<IHouse>(xml);

            Assert.AreEqual(testHouse, deserialized);
        }
    }
}
