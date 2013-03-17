using AssessorsAdapter;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sonneville.Utilities.Persistence;

namespace AssessorsAdapterTest
{
    [TestClass]
    public class HouseAppTest
    {
        private Mock<IRepository<string, IHouse>> _masterRepositoryMock;
        private readonly HouseFactory _factory = new HouseFactory();

        [TestInitialize]
        public void Initialize()
        {
            _masterRepositoryMock = new Mock<IRepository<string, IHouse>>();
        }

        [TestMethod]
        public void ConstructorArgumentSetsRepository()
        {
            var app = GetTestObject();

            Assert.AreEqual(_masterRepositoryMock.Object, app.Repository);
        }

        [TestMethod]
        public void GetHouseReturnsCorrectHouse()
        {
            var house = House1;
            _masterRepositoryMock.Setup(x => x.Fetch(house.Address)).Returns(house);

            var app = GetTestObject();
            var result = app.GetHouse(house.Address);

            Assert.AreEqual(house, result);
        }

        private HouseApp GetTestObject()
        {
            return new HouseApp(_masterRepositoryMock.Object);
        }

        private IHouse House1
        {
            get { return GetHouse(Resources._6324_Wilcot_Ct, Resources._6324_Wilcot_Ct___taxes); }
        }

        private IHouse House2
        {
            get { return GetHouse(Resources._9260_NW_36th_St, Resources._9260_NW_36th_St___taxes); }
        }

        private IHouse GetHouse(string houseHtml, string taxesHtml)
        {
            var houseDoc = new HtmlDocument();
            houseDoc.LoadHtml(houseHtml);
            var taxesDoc = new HtmlDocument();
            taxesDoc.LoadHtml(taxesHtml);
            return _factory.ConstructHouse(houseDoc, taxesDoc);
        }
    }
}
