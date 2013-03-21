using AssessorsAdapter;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class OtherTests : HouseTestBase
    {
        private readonly HouseFactory _factory = new HouseFactory();
        private static IHouse _noResultsHouse;
        private static IHouse _duplicateAddresses;
        private static IHouse _missingFireplaces;

        [TestInitialize]
        public void Initialize()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Resources._123_Fake_St);
            _noResultsHouse = _factory.ConstructHouse(doc);

            doc = new HtmlDocument();
            doc.LoadHtml(Resources._9823_Laguna_Dr);
            _duplicateAddresses = _factory.ConstructHouse(doc);

            doc = new HtmlDocument();
            doc.LoadHtml(Resources._4624_Tamara_Ln);
            _missingFireplaces = _factory.ConstructHouse(doc);
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

        [TestMethod]
        public void MissingFireplacesTag()
        {
            Assert.AreEqual(0, _missingFireplaces.Fireplaces);
        }
    }
}
