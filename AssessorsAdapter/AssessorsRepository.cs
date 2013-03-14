using AssessorsAdapter.Persistence;

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
            throw new System.NotImplementedException();
        }

        public bool ContainsValue(IHouse value)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}