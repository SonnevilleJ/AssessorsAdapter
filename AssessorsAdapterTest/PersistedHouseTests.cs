using System.Linq;
using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class PersistedHouseTests
    {
        private static readonly IHouse AssessorsHouse = ConstructHouse(Address);

        private const string Address = "6324 Wilcot Ct";

        private static IHouse ConstructHouse(string address)
        {
            var house = new AssessorsHouse();
            house.FetchData(address);
            return house;
        }

        [TestMethod]
        public void ConvertFromAssessorsHouse()
        {
            var house = PersistedHouse.FromIHouse(AssessorsHouse);

            Assert.IsNotNull(house);
        }

        [TestMethod]
        public void ConvertionCopiesAllProperties()
        {
            var house = PersistedHouse.FromIHouse(AssessorsHouse);

            if (typeof (IHouse).GetProperties().Any(propertyInfo => !propertyInfo.GetValue(house).Equals(propertyInfo.GetValue(AssessorsHouse)))) Assert.Fail();
        }
    }
}
