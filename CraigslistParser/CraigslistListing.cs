using System;

namespace CraigslistParser
{
    public class CraigslistListing
    {
        public string Id { get; set; }
        public string CraigslistCategoryText { get; set; }
        public string ListingTitle { get; set; }
        public string ItemPageUrl { get; set; }
        public DateTime? PostingDate { get; set; }
        public bool HasImage { get; set; }
        public string ImageUrl { get; set; }
        public bool HasMapLocation { get; set; }
        public string LocationRegion { get; set; }
        public string LocationLatitude { get; set; }
        public string LocationLongitude { get; set; }
    }
}
