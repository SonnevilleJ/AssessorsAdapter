﻿using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class OtherTests : HouseTestBase
    {
        private static readonly House DuplicateAddresses = ConstructHouse("9823 Laguna Dr");
        private static readonly House NoResultsHouse = ConstructHouse("123 Fake St");

        [TestMethod]
        public void NoResultsTest()
        {
            Assert.IsTrue(NoResultsHouse.NoRecordsFound);
        }

        [TestMethod]
        public void NoResultsSetsDataAvailable()
        {
            Assert.IsFalse(NoResultsHouse.DataAvailable);
        }

        [TestMethod]
        public void MultipleResultsTest()
        {
            Assert.IsTrue(DuplicateAddresses.MultipleRecordsFound);
        }

        [TestMethod]
        public void DuplicateAddressesDataAvailable()
        {
            Assert.IsFalse(DuplicateAddresses.DataAvailable);
        }
    }
}
