using AssessorsAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class AssessorsRepositoryTest
    {
        [TestMethod]
        public void SaveDoesNothing()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);
            var houseMock = new Mock<IHouse>();

            repo.Save(houseMock.Object);

            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>()), Times.Never());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void DeleteDoesNothing()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);
            var houseMock = new Mock<IHouse>();

            repo.Delete(houseMock.Object);

            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>()), Times.Never());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void EmptyDoesNothing()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            repo.Empty();

            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>()), Times.Never());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void FetchCallsHouseFactory()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);
            const string address = "123 My Street";
            
            repo.Fetch(address);

            factoryMock.Verify(x => x.ConstructHouse(address), Times.Once());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void FetchReturnsHouse()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);
            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);
            factoryMock.Setup(x => x.ConstructHouse(address)).Returns(houseMock.Object);
            
            var result = repo.Fetch(address);

            Assert.AreEqual(address, result.Address);
        }

        [TestMethod]
        public void ContainsKeyGetsHouseToVerifyContain()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);

            repo.ContainsKey(address);

            factoryMock.Verify(x => x.ConstructHouse(address), Times.Once());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void ContainsKeyWhenFactoryReturnsObject()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);
            factoryMock.Setup(x => x.ConstructHouse(address)).Returns(houseMock.Object);

            var result = repo.ContainsKey(address);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsKeyWhenFactoryReturnsEmptyObject()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            const string address = "123 My Street";
            factoryMock.Setup(x => x.ConstructHouse(address)).Returns(null as IHouse);

            var result = repo.ContainsKey(address);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsValueGetsHouseToVerifyContain()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);

            repo.ContainsValue(houseMock.Object);

            factoryMock.Verify(x => x.ConstructHouse(address), Times.Once());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }

        [TestMethod]
        public void ContainsValueWhenFactoryReturnsObject()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);
            factoryMock.Setup(x => x.ConstructHouse(address)).Returns(houseMock.Object);

            var result = repo.ContainsValue(houseMock.Object);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsValueWhenFactoryReturnsEmptyObject()
        {
            var factoryMock = new Mock<IHouseFactory>();
            var repo = new AssessorsRepository(factoryMock.Object);

            var houseMock = new Mock<IHouse>();
            const string address = "123 My Street";
            houseMock.SetupGet(house => house.Address).Returns(address);
            factoryMock.Setup(x => x.ConstructHouse(address)).Returns(null as IHouse);

            var result = repo.ContainsValue(houseMock.Object);

            Assert.IsFalse(result);
        }
    }
}
