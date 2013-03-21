using System.Globalization;
using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest
{
    public class HouseTestBase
    {
        protected static void VerifyAddressMatches(string address, IHouse house)
        {
            Assert.AreEqual(address, house.Address, true);
        }

        protected static void VerifyCityMatches(string city, IHouse house)
        {
            Assert.AreEqual(city, house.City, true);
        }

        protected static void VerifyZipMatches(int zip, IHouse house)
        {
            VerifyZipMatches(zip.ToString(CultureInfo.InvariantCulture), house);
        }

        private static void VerifyZipMatches(string zip, IHouse house)
        {
            Assert.AreEqual(zip, house.Zip);
        }

        protected static void VerifyAssessmentMatches(int assessment, IHouse house)
        {
            Assert.AreEqual(assessment, house.AssessmentTotal);
        }

        protected static void VerifyLandMatches(int land, IHouse house)
        {
            Assert.AreEqual(land, house.Land);
        }

        protected static void VerifyTsflaMatches(int tsfla, IHouse house)
        {
            Assert.AreEqual(tsfla, house.TSFLA);
        }

        protected static void VerifyBasementMatches(int bsmtArea, IHouse house)
        {
            Assert.AreEqual(bsmtArea, house.BsmtArea);
        }

        protected static void VerifyYearBuiltMatches(int yearBuilt, IHouse house)
        {
            Assert.AreEqual(yearBuilt, house.YearBuilt);
        }

        protected static void VerifyFireplacesMatches(int fireplaces, IHouse house)
        {
            Assert.AreEqual(fireplaces, house.Fireplaces);
        }

        protected static void VerifyTaxesMatch(double taxes, IHouse house)
        {
            Assert.AreEqual(taxes, house.GrossTaxes);
        }
    }
}