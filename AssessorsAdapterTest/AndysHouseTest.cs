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
            var housePage = new HtmlDocument();
            housePage.LoadHtml(Resources._9260_NW_36th_St);
            var taxPage = new HtmlDocument();
            taxPage.LoadHtml(Resources._9260_NW_36th_St___taxes);
            _andysHouse = HouseFactory.ConstructHouse(housePage, taxPage);
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