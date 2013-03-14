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
        private CachingRepository<int, string> _cachingRepository;

        [TestInitialize]
        public void Initialize()
        {
            _cacheMock = new Mock<IRepository<int, string>>();
            _masterMock = new Mock<IRepository<int, string>>();
            _cachingRepository = new CachingRepository<int, string>(_masterMock.Object, _cacheMock.Object);
        }

        [TestMethod]
        public void SavesToCache()
        {
            const int key = 1;
            const string value = "Hello";

            _cachingRepository.Save(key, value);

            _cacheMock.Verify(repo => repo.Save(key, value), Times.Once());
            _masterMock.Verify(repo => repo.Save(It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void ContainsKeyWhenCacheContains()
        {
            const int key = 1;
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);
            
            _cachingRepository.ContainsKey(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _masterMock.Verify(repo => repo.ContainsKey(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void ContainsKeyWhenCacheDoesNotContain()
        {
            const int key = 1;
            _masterMock.Setup(repo => repo.ContainsKey(key)).Returns(true);
            
            _cachingRepository.ContainsKey(key);

            _cacheMock.Verify(repo => repo.ContainsKey(key), Times.Once());
            _masterMock.Verify(repo => repo.ContainsKey(key), Times.Once());
        }

        [TestMethod]
        public void ContainsValueWhenCacheContains()
        {
            const string value = "John";
            _cacheMock.Setup(repo => repo.ContainsValue(value)).Returns(true);
            
            _cachingRepository.ContainsValue(value);

            _cacheMock.Verify(repo => repo.ContainsValue(value), Times.Once());
            _masterMock.Verify(repo => repo.ContainsValue(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void ContainsValueWhenCacheDoesNotContain()
        {
            const string value = "John";
            _masterMock.Setup(repo => repo.ContainsValue(value)).Returns(true);
            
            _cachingRepository.ContainsValue(value);

            _cacheMock.Verify(repo => repo.ContainsValue(value), Times.Once());
            _masterMock.Verify(repo => repo.ContainsValue(value), Times.Once());
        }

        [TestMethod]
        public void DeleteWhenCacheDoesContain()
        {
            const int key = 1;

            _cachingRepository.Delete(key);

            _cacheMock.Verify(repo => repo.Delete(key), Times.Once());
            _masterMock.Verify(repo => repo.Delete(key), Times.Once());
        }

        [TestMethod]
        public void DeleteWhenCacheDoesNotContain()
        {
            const int key = 1;
            _cacheMock.Setup(repo => repo.ContainsKey(key)).Returns(true);

            _cachingRepository.Delete(key);

            _cacheMock.Verify(repo => repo.Delete(key), Times.Once());
            _masterMock.Verify(repo => repo.Delete(key), Times.Once());
        }
    }
}
