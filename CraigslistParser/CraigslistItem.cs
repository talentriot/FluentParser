using System;
using System.Collections.Generic;

namespace CraigslistParser
{
    public class CraigslistItem
    {
        public string Id { get; set; }
        public string CraigslistCategoryText { get; set; }
        public string ListingTitle { get; set; }
        public string ItemPageUrl { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool HasImage { get; set; }
        public string ImageUrl { get; set; }
        public bool HasMapLocation { get; set; }
        public string Address { get; set; }
        public string GoogleMapsUrl { get; set; }
        public string YahooMapsUrl { get; set; }
        public string LocationRegion { get; set; }
        public string PostingBody { get; set; }
        public Dictionary<string, string> Tags { get; set; }
    }
}
