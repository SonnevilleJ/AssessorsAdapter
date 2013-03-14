using HtmlAgilityPack;

namespace AssessorsAdapter
{
    public interface IHouseFactory
    {
        IHouse ConstructHouse(string address);
        IHouse ConstructHouse(string address, string city, bool photo, bool map);
        IHouse Clone(IHouse assessorsHouse);
    }
}