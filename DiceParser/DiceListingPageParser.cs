using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceListingPageParser
    {
        public List<DiceListing> GetAllFor(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var allHtmlListing = documentRoot.QueryManyFromSelectorChain("#SRmCol", ".summary", ".STDsrRes").ToList();
            var remainingHtmlListing = documentRoot.QueryManyFromSelectorChain("#SRmCol", ".summary", ".gold").ToList();
            allHtmlListing.AddRange(remainingHtmlListing);

            return allHtmlListing.Select(GetListingFromHtmlRow).ToList();
        }

        private static DiceListing GetListingFromHtmlRow(HtmlNode htmlListing)
        {

            var jobTitle = htmlListing.QueryXth(1, "td").QuerySelector("a").InnerText;
            var company = htmlListing.QueryXth(2, "td").QuerySelector("a").InnerText;
            var location = htmlListing.QueryXth(3, "td").InnerText;
            var postingUrl = htmlListing.GetAttributeValue("href", null);

            var datePostedString = htmlListing.QuerySelector("td:last-child").InnerText;
            var datePosted = DateTime.Parse(datePostedString);

            var listing = new DiceListing
            {
                JobTitle = jobTitle,
                Company = company,
                Location = location,
                PostingUrl = postingUrl,
                DatePosted = datePosted
            };
            return listing;
        }
    }
}
