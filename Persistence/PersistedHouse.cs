using System.Diagnostics;
using AssessorsAdapter;

namespace Persistence
{
    public class PersistedHouse : IHouse
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

        public string HomeUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int AssessmentTotal { get; set; }
        public int Land { get; set; }
        public bool MultipleRecordsFound { get; set; }
        public bool NoRecordsFound { get; set; }
        public int TSFLA { get; set; }
        public bool DataAvailable { get; set; }
        public int BsmtArea { get; set; }
        public int YearBuilt { get; set; }
        public int Fireplaces { get; set; }
        public decimal GrossTaxes { get; set; }
        public void OpenWebPage()
        {
            Process.Start("chrome", HomeUrl);
        }

        public static PersistedHouse FromIHouse(IHouse assessorsHouse)
        {
            return new PersistedHouse(assessorsHouse);
        }
    }
}