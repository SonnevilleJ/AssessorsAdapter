using AssessorsAdapter;
using HtmlAgilityPack;
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
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<HtmlDocument>()), Times.Never());
            factoryMock.Verify(x => x.ConstructHouse(It.IsAny<HtmlDocument>(), It.IsAny<HtmlDocument>()), Times.Never());
            factoryMock.Verify(x => x.Clone(It.IsAny<IHouse>()), Times.Never());
        }
    }
}
