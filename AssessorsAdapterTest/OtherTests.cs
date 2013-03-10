using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class OtherTests : HouseTestBase
    {
        private static IHouse _noResultsHouse;
        private static IHouse _duplicateAddresses;

        [TestInitialize]
        public void Initialize()
        {
            _noResultsHouse = HouseFactory.ConstructHouse("123 Fake St");
            _duplicateAddresses = HouseFactory.ConstructHouse("9823 Laguna Dr");
        }

        [TestMethod]
        public void NoResultsTest()
        {
            Assert.IsTrue(_noResultsHouse.NoRecordsFound);
        }

        [TestMethod]
        public void NoResultsSetsDataAvailable()
        {
            Assert.IsFalse(_noResultsHouse.DataAvailable);
        }

        [TestMethod]
        public void MultipleResultsTest()
        {
            Assert.IsTrue(_duplicateAddresses.MultipleRecordsFound);
        }

        [TestMethod]
        public void DuplicateAddressesDataAvailable()
        {
            Assert.IsFalse(_duplicateAddresses.DataAvailable);
        }
    }
}
