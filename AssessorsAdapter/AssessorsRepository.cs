using Sonneville.Utilities.Persistence;

namespace AssessorsAdapter
{
    public class AssessorsRepository : IRepository<string, IHouse>
    {
        private readonly IHouseFactory _houseFactory;

        public AssessorsRepository(IHouseFactory houseFactory)
        {
            _houseFactory = houseFactory;
        }

        public void Save(string key, IHouse value)
        {
        }

        public void Save(IHouse house)
        {
            Save(house.Address, house);
        }

        public void Delete(string key)
        {
        }

        public void Delete(IHouse house)
        {
            Delete(house.Address);
        }

        public bool ContainsValue(IHouse value)
        {
            return ContainsKey(value.Address);
        }

        public bool ContainsKey(string key)
        {
            return Fetch(key) != null;
        }

        public IHouse Fetch(string key)
        {
            return _houseFactory.ConstructHouse(key);
        }

        public void Empty()
        {
        }
    }
}