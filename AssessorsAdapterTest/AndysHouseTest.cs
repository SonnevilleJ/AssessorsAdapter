using AssessorsAdapter;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class AndysHouseTest : HouseTestBase
    {
        private const string Address = "9260 NW 36th St";
        private const string City = "Polk City";
        private const int Zip = 50226;

        private static IHouse _andysHouse;

        [TestInitialize]
        public void Initialize()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Resources._9260_NW_36th_St);
            _andysHouse = HouseFactory.ConstructHouse(doc, Address, City);
        }

        [TestMethod]
        public void AndysAddressMatches()
        {
            VerifyAddressMatches(Address, _andysHouse);
        }

        [TestMethod]
        public void AndysCityMatches()
        {
            VerifyCityMatches(City, _andysHouse);
        }

        [TestMethod]
        public void AndysZipMatches()
        {
            VerifyZipMatches(Zip, _andysHouse);
        }
    }
}