using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace CraigslistParser
{
    public class CraigslistListingPageParser
    {
        public IEnumerable<CraigslistListing> GetAllFor(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var breadcrumbs = documentRoot.QueryManyFromSelectorChain("header.bchead", ".crumb");
            var categoryText = breadcrumbs.Last().QuerySelector("a").InnerText;

            var allHtmlListings = documentRoot.QueryManyFromSelectorChain("#toc_rows", ".content", ".row");

            return allHtmlListings.Select(htmlListing => GetListingFromHtmlRow(htmlListing, categoryText));
        }

        private static CraigslistListing GetListingFromHtmlRow(HtmlNode htmlListing, string categoryText)
        {
            var latitude = htmlListing.GetAttributeValue("data-latitude", null);
            var longitude = htmlListing.GetAttributeValue("data-longitude", null);
            var id = htmlListing.GetAttributeValue("data-pid", null);

            var anchor = htmlListing.QuerySelector("a");
            var itemPageUrl = anchor.GetAttributeValue("href", null);
            var image = anchor.QuerySelector("img");
            var hasImage = image != null;
            var imageUrl = hasImage ? image.GetAttributeValue("src", null) : null;
            var plSection = htmlListing.QuerySelector("span.pl");
            var dateString = plSection.QuerySelector(".date").InnerText;;
            var dayComponent = dateString.Split(' ')[1];
            var monthComponent = dateString.Split(' ')[0];
            var year = DateTime.Now.Year;
            DateTime? parsedDate = null;
            if (!string.IsNullOrWhiteSpace(dayComponent) && !string.IsNullOrWhiteSpace(monthComponent))
            {
                var month = monthComponent.ShortMonthStringToInt();
                parsedDate = new DateTime(year, month, dayComponent.AsInt());
            }
            var listingTitle = plSection.QuerySelector("a").InnerText;

            var locationSection = htmlListing.QuerySelector("span.l2");
            var locationSpan = locationSection.TryQuerySelectorChain("span.pnr", "small");
            string location = null;
            if (locationSpan != null)
            {
                location = locationSpan.InnerText;
                location = location.Trim().Trim(new[] {'(', ')'});
            }

            var hasMap = locationSection.QueryFromSelectorChain("span.px", "span.p").HasChildNodes;

            var listing = new CraigslistListing
            {
                CraigslistCategoryText = categoryText,
                HasImage = hasImage,
                HasMapLocation = hasMap,
                Id = id,
                ImageUrl = imageUrl,
                ItemPageUrl = itemPageUrl,
                ListingTitle = listingTitle,
                PostingDate = parsedDate,
                LocationRegion = location,
                LocationLatitude = latitude,
                LocationLongitude = longitude
            };
            return listing;
        }
    }
}
