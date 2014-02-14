using System;
using System.Linq;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace CraigslistParser
{
    public class CraigslistItemPageParser
    {
        public CraigslistItem ParseItemFromPage(HtmlDocument craigslistItemPage)
        {
            var documentRoot = craigslistItemPage.DocumentNode;
            
            var breadcrumbs = documentRoot.QueryManyFromSelectorChain("header.bchead", ".crumb");
            var categoryLink = breadcrumbs.Last().QuerySelector("a");
            var categoryText = categoryLink.InnerText;
            var categoryUrl = categoryLink.GetAttributeValue("href", null);

            var postingInfoSection = documentRoot.QuerySelector("div.postinginfos");
            var postingInfoBlocks = postingInfoSection.QuerySelectorAll(".postinginfo").ToList();
            var postIdText = postingInfoBlocks.First(x => x.InnerText.Contains("post id: ")).InnerText;
            var postId = Regex.Match(postIdText, @"\d+").Value;

            var postDateText = postingInfoBlocks.First(x => x.InnerText.Contains("posted: "))
                    .QuerySelector("time")
                    .GetAttributeValue("datetime", null);
            var updateDateText = postingInfoBlocks.First(x => x.InnerText.Contains("updated: "))
                    .QuerySelector("time")
                    .GetAttributeValue("datetime", null);

            var bodySection = documentRoot.QuerySelector("section.body");
            var listingTitleAndLocationRegion = bodySection.QuerySelector("h2.postingtitle").InnerText;
            var listingTitleComponents = listingTitleAndLocationRegion.Split('(');
            var listingTitle = listingTitleComponents[0].Trim();
            string locationRegion = null;
            if (listingTitleComponents.Length > 1)
            {
                locationRegion = listingTitleComponents[1].Trim().Replace(")", "");
            }

            var userBodySection = documentRoot.QuerySelector("section.userbody");
            var imageTag = userBodySection.TryQuerySelectorChain("figure.iw", "div#ci", "img#iwi");
            var imageUrl = imageTag == null ? null : imageTag.GetAttributeValue("src", null);

            var body = documentRoot.QuerySelector("section#postingbody").InnerText;

            var cltagsSection = documentRoot.QuerySelector("section.cltags");
            var addressTag = cltagsSection.QuerySelector("p.mapaddress");
            var hasMap = addressTag != null;
            string address = null, googleMapsUrl = null, yahooMapsUrl = null;
            if (hasMap)
            {
                address = addressTag.FirstChild.InnerText.Trim();
                var mapUrls = addressTag.QueryManyFromSelectorChain("small", "a");
                foreach (var mapUrl in mapUrls)
                {
                    var innerText = mapUrl.InnerText;
                    if (innerText.Contains("google map"))
                    {
                        googleMapsUrl = mapUrl.GetAttributeValue("href", null);
                    }
                    else if (innerText.Contains("yahoo map"))
                    {
                        yahooMapsUrl = mapUrl.GetAttributeValue("href", null);
                    }
                }
            }

            var parsedItem = new CraigslistItem
            {
                Address = address,
                CraigslistCategoryText = categoryText,
                GoogleMapsUrl = googleMapsUrl,
                HasImage = imageTag == null,
                HasMapLocation = hasMap,
                Id = postId,
                ImageUrl = imageUrl,
                ItemPageUrl = string.Format("{0}{1}.html", categoryUrl, postId),
                LastUpdated = DateTime.Parse(updateDateText),
                ListingTitle = listingTitle,
                LocationRegion = locationRegion,
                PostingBody = body,
                PostingDate = DateTime.Parse(postDateText),
                YahooMapsUrl = yahooMapsUrl
//                LocationLatitude = latitude,
//                LocationLongitude = longitude
            };
            return parsedItem;
        }
    }
}
