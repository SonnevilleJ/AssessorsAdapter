using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class AndysHouseTest : HouseTestBase
    {
        private const string AndysAddress = "9260 NW 36th St";
        
        private static readonly IHouse AndysHouse = ConstructHouse(AndysAddress);

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