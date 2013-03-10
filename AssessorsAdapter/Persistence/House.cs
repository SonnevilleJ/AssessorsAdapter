namespace AssessorsAdapter.Persistence
{
    public class House : HouseBase
    {
        public House()
        {
        }

        public House(IHouse house)
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
    }
}