using AssessorsAdapter.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AssessorsAdapterTest.Persistence
{
    [TestClass]
    public class CachingRepositoryTests
    {
        private Mock<IRepository<int, string>> _cacheMock;
        private Mock<IRepository<int, string>> _masterMock;
        private CachingRepository<int, string> _testObject;

        [TestInitialize]
        public void Initialize()
        {
            _cacheMock = new Mock<IRepository<int, string>>();
            _masterMock = new Mock<IRepository<int, string>>();
            _testObject = new CachingRepository<int, string>(_masterMock.Object, _cacheMock.Object);
        }

        [TestMethod]
        public void Save()
        {
            const int key = 1;
            const string value = "Hello";

            _testObject.Save(key, value);

            _cacheMock.Verify(repo => repo.Save(key, value), Times.Once());
            _masterMock.Verify(repo => repo.Save(key, value), Times.Once());
        }

        [TestMethod]
        public void ContainsKeyWhenCacheContains()
        {
            const int key = 1;
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);
            
            _testObject.ContainsKey(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _masterMock.Verify(repo => repo.ContainsKey(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void ContainsKeyWhenCacheDoesNotContain()
        {
            const int key = 1;
            _masterMock.Setup(repo => repo.ContainsKey(key)).Returns(true);
            
            _testObject.ContainsKey(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _masterMock.Verify(repo => repo.ContainsKey(key), Times.Once());
        }

        [TestMethod]
        public void ContainsValueWhenCacheContains()
        {
            const string value = "John";
            _cacheMock.Setup(repo => repo.ContainsValue(value)).Returns(true);
            
            _testObject.ContainsValue(value);

            _cacheMock.Verify(repo => repo.ContainsValue(value), Times.Once());
            _masterMock.Verify(repo => repo.ContainsValue(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void ContainsValueWhenCacheDoesNotContain()
        {
            const string value = "John";
            _masterMock.Setup(repo => repo.ContainsValue(value)).Returns(true);
            
            _testObject.ContainsValue(value);

            _cacheMock.Verify(repo => repo.ContainsValue(value), Times.Once());
            _masterMock.Verify(repo => repo.ContainsValue(value), Times.Once());
        }

        [TestMethod]
        public void DeleteWhenCacheDoesContain()
        {
            const int key = 1;

            _testObject.Delete(key);

            _cacheMock.Verify(repo => repo.Delete(key), Times.Once());
            _masterMock.Verify(repo => repo.Delete(key), Times.Once());
        }

        [TestMethod]
        public void DeleteWhenCacheDoesNotContain()
        {
            const int key = 1;
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);

            _testObject.Delete(key);

            _cacheMock.Verify(repo => repo.Delete(key), Times.Once());
            _masterMock.Verify(repo => repo.Delete(key), Times.Once());
        }

        [TestMethod]
        public void FetchChecksCacheFirst()
        {
            const int key = 1;
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);

            _testObject.Fetch(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _masterMock.Verify(repo => repo.Fetch(It.IsAny<int>()), Times.Never());
            _masterMock.Verify(repo => repo.ContainsKey(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void FetchFetchesFromCacheIfCacheContainsKey()
        {
            const int key = 1;
            const string value = "John";
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);
            _cacheMock.Setup(repo => repo.Fetch(key)).Returns(value);

            var result = _testObject.Fetch(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _cacheMock.Verify(repo => repo.Fetch(key), Times.Once());
            _masterMock.Verify(repo => repo.Fetch(It.IsAny<int>()), Times.Never());
            _masterMock.Verify(repo => repo.ContainsKey(It.IsAny<int>()), Times.Never());

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void FetchFetchesFromMasterIfCacheDoesNotContainKey()
        {
            const int key = 1;
            const string value = "John";
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(false);
            _masterMock.Setup(repo => repo.Fetch(key)).Returns(value);

            var result = _testObject.Fetch(key);

            _masterMock.Verify(repo => repo.Fetch(key), Times.Once());

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void EmptyCallsCacheEmpty()
        {
            _testObject.Empty();

            _cacheMock.Verify(repo => repo.Empty(), Times.Once());
            _masterMock.Verify(repo => repo.Empty(), Times.Never());
        }
    }
}
