using System;

namespace AssessorsAdapter
{
    public interface IHouse : IEquatable<IHouse>
    {
        string Address { get; set; }
        string City { get; set; }
        string Zip { get; set; }
        int AssessmentTotal { get; set; }
        int Land { get; set; }
        bool MultipleRecordsFound { get; set; }
        bool NoRecordsFound { get; set; }
        int TSFLA { get; set; }
        bool DataAvailable { get; set; }
        int BsmtArea { get; set; }
        int YearBuilt { get; set; }
        int Fireplaces { get; set; }
        decimal GrossTaxes { get; set; }
    }
}