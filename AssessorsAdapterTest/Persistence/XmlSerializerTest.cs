using System.Collections.Generic;
using System.Linq;
using AssessorsAdapter.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class XmlSerializerTest
    {
        [TestMethod]
        public void SerializeString()
        {
            const string testObject = "Test";
            var serializedObject = XmlSerializer.SerializeToXml(testObject);
            var deserializedObject = XmlSerializer.DeserializeFromXml<string>(serializedObject);

            Assert.AreEqual(testObject, deserializedObject);
        }

        [TestMethod]
        public void SerializeListOfInt()
        {
            var list = new List<int> {1, 2, 3};
            var serializedObject = XmlSerializer.SerializeToXml(list);
            var deserializedObject = XmlSerializer.DeserializeFromXml<List<int>>(serializedObject);

            CollectionAssert.AreEquivalent(list, deserializedObject);
        }

        [TestMethod]
        public void SerializeListOfString()
        {
            var list = new List<string> {"one", "two", "three"};
            var serializedObject = XmlSerializer.SerializeToXml(list);
            var deserializedObject = XmlSerializer.DeserializeFromXml<List<string>>(serializedObject);

            CollectionAssert.AreEquivalent(list, deserializedObject);
        }
    }
}
