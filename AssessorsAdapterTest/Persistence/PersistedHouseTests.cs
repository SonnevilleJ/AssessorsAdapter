using System.Linq;
using AssessorsAdapter;
using AssessorsAdapter.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class PersistedHouseTests
    {
        private static readonly IHouse AssessorsHouse = HouseTest.TestHouse;

        [TestMethod]
        public void ConvertFromAssessorsHouse()
        {
            var house = HouseFactory.Clone(AssessorsHouse);

            Assert.IsNotNull(house);
        }

        [TestMethod]
        public void ConvertionCopiesAllProperties()
        {
            var house = HouseFactory.Clone(AssessorsHouse);

            if (typeof (IHouse).GetProperties().Any(propertyInfo => !propertyInfo.GetValue(house).Equals(propertyInfo.GetValue(AssessorsHouse)))) Assert.Fail();
        }

        [TestMethod]
        public void PersistedHouseEqualsAssessorsHouse()
        {
            var house = HouseFactory.Clone(AssessorsHouse);

            Assert.IsTrue(house.Equals(AssessorsHouse));
        }

        [TestMethod]
        public void SerializationTest()
        {
            var target = HouseFactory.Clone(AssessorsHouse);
            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<House>(xml);

            var properties = target.GetType().GetProperties();
            foreach (var propertyInfo in properties.Where(propertyInfo => propertyInfo.GetIndexParameters().Length == 0))
            {
                Assert.AreEqual(propertyInfo.GetValue(target), propertyInfo.GetValue(result));
            }
        }
    }
}
