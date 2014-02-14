using System;
using System.Collections.Generic;

namespace DiceParser
{
    public class DiceResults
    {
        public IEnumerable<DiceResultListing> Listings { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class DiceResultListing
    {
        public string JobTitle { get; set; }
        public Uri DetailsPage { get; set; }
        public string Company { get; set; }
        public string JobLocation { get; set; }
        public DateTime? PostedDate { get; set; }
    }
}
