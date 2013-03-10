﻿using AssessorsAdapter;
using HtmlAgilityPack;
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
            var doc = new HtmlDocument();
            doc.LoadHtml(Resources._123_Fake_St);
            _noResultsHouse = HouseFactory.ConstructHouse(doc);

            doc = new HtmlDocument();
            doc.LoadHtml(Resources._9823_Laguna_Dr);
            _duplicateAddresses = HouseFactory.ConstructHouse(doc);
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
