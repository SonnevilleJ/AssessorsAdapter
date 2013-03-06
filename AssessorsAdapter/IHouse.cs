namespace AssessorsAdapter
{
    public interface IHouse
    {
        string HomeUrl { get; }
        string Address { get; }
        string City { get; }
        string Zip { get; }
        int AssessmentTotal { get; }
        int Land { get; }
        bool MultipleRecordsFound { get; }
        bool NoRecordsFound { get; }
        int TSFLA { get; }
        bool DataAvailable { get; }
        int BsmtArea { get; }
        int YearBuilt { get; }
        int Fireplaces { get; }
        decimal GrossTaxes { get; }
        void OpenWebPage();
    }
}