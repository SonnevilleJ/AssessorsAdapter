using System.Globalization;
using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    public class HouseTestBase
    {
        protected static void VerifyAddressMatches(string address, House house)
        {
            Assert.AreEqual(address, house.Address, true);
        }

        protected static void VerifyCityMatches(string city, House house)
        {
            Assert.AreEqual(city, house.City, true);
        }

        protected static void VerifyZipMatches(int zip, House house)
        {
            VerifyZipMatches(zip.ToString(CultureInfo.InvariantCulture), house);
        }

        private static void VerifyZipMatches(string zip, House house)
        {
            Assert.AreEqual(zip, house.Zip);
        }

        protected static void VerifyAssessmentMatches(int assessment, House house)
        {
            Assert.AreEqual(assessment, house.AssessmentTotal);
        }

        protected static void VerifyLandMatches(int land, House house)
        {
            Assert.AreEqual(land, house.Land);
        }

        protected static void VerifyTsflaMatches(int tsfla, House house)
        {
            Assert.AreEqual(tsfla, house.TSFLA);
        }

        protected static void VerifyBasementMatches(int bsmtArea, House house)
        {
            Assert.AreEqual(bsmtArea, house.BsmtArea);
        }

        protected static House ConstructHouse(string address)
        {
            var house = new House();
            house.FetchData(address);
            return house;
        }
    }
}