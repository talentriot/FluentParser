using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceListingPageParser 
    {
        public IEnumerable<DiceListing> GetAllListingsFor(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var allHtmlListings = documentRoot.QueryManyFromSelectorChain(".STDsrRes");

            return allHtmlListings.Select(htmlListing => GetListingFromHtmlRow(htmlListing));
        }

        private static DiceListing GetListingFromHtmlRow(HtmlNode htmlListing)
        {
            var linkNodes = htmlListing.QuerySelectorAll("a");

            var jobTitleNode = linkNodes.Where(n => n.Attributes.Count == 1).Single();
            
            var jobTitle = jobTitleNode.InnerText;
            var jobUrl = jobTitleNode.Attributes.Where(attr => attr.Name.Contains("href")).Select(attr => attr.Value).FirstOrDefault();

            var companyNameNode = linkNodes.Where(n => n.Attributes.Where(attr => attr.Name == "title").Any()).Single();

            var companyNameNode = htmlListing.QuerySelector("a");
            var listing = new DiceListing()
            {
                JobTitle = jobTitle,
                JobTitleUrl = jobUrl,
                CompanyName = "",
                CompanyNameUrl = "",
                Location = "",
                LocationUrl = "",
                DatePosted = new DateTime()
            };
            return listing;
        }
    }
}
