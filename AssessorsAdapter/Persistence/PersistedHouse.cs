namespace AssessorsAdapter.Persistence
{
    public class PersistedHouse : HouseBase
    {
        public PersistedHouse()
        {
        }

        private PersistedHouse(IHouse house)
        {
            HomeUrl = house.HomeUrl;
            Address = house.Address;
            City = house.City;
            Zip = house.Zip;
            AssessmentTotal = house.AssessmentTotal;
            Land = house.Land;
            MultipleRecordsFound = house.MultipleRecordsFound;
            NoRecordsFound = house.NoRecordsFound;
            TSFLA = house.TSFLA;
            DataAvailable = house.DataAvailable;
            BsmtArea = house.BsmtArea;
            YearBuilt = house.YearBuilt;
            Fireplaces = house.Fireplaces;
            GrossTaxes = house.GrossTaxes;
        }

        public static PersistedHouse FromIHouse(IHouse assessorsHouse)
        {
            return new PersistedHouse(assessorsHouse);
        }
    }
}