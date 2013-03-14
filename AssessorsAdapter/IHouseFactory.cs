using HtmlAgilityPack;

namespace AssessorsAdapter
{
    public interface IHouseFactory
    {
        IHouse ConstructHouse(string address);
        IHouse ConstructHouse(string address, string city, bool photo, bool map);
        IHouse ConstructHouse(HtmlDocument housePage);
        IHouse ConstructHouse(HtmlDocument housePage, HtmlDocument taxPage);
        IHouse Clone(IHouse assessorsHouse);
    }
}