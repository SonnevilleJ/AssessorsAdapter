﻿using System.Runtime.InteropServices;

namespace AssessorsAdapter
{
#if DEBUG
    [ClassInterface(ClassInterfaceType.AutoDual)]
#endif
    [ComVisible(true)]
    public class House : IHouse
    {
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

        public double GrossTaxes { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IHouse other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Address == other.Address &&
                   City == other.City &&
                   Zip == other.Zip &&
                   AssessmentTotal == other.AssessmentTotal &&
                   Land == other.Land &&
                   MultipleRecordsFound == other.MultipleRecordsFound &&
                   NoRecordsFound == other.NoRecordsFound &&
                   TSFLA == other.TSFLA &&
                   DataAvailable == other.DataAvailable &&
                   BsmtArea == other.BsmtArea &&
                   YearBuilt == other.YearBuilt &&
                   Fireplaces == other.Fireplaces &&
                   GrossTaxes == other.GrossTaxes;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IHouse);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Zip != null ? Zip.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AssessmentTotal;
                hashCode = (hashCode * 397) ^ Land;
                hashCode = (hashCode * 397) ^ MultipleRecordsFound.GetHashCode();
                hashCode = (hashCode * 397) ^ NoRecordsFound.GetHashCode();
                hashCode = (hashCode * 397) ^ TSFLA;
                hashCode = (hashCode * 397) ^ DataAvailable.GetHashCode();
                hashCode = (hashCode * 397) ^ BsmtArea;
                hashCode = (hashCode * 397) ^ YearBuilt;
                hashCode = (hashCode * 397) ^ Fireplaces;
                hashCode = (hashCode * 397) ^ GrossTaxes.GetHashCode();
                return hashCode;
            }
        }
    }
}