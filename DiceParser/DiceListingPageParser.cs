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
            var ancorNodes = htmlListing.QuerySelectorAll("a");

            var jobTitleNode = ancorNodes.Single(n => GetHrefAttributeFromNode(n).Value.Contains("/job/result/"));
            
            var jobTitle = jobTitleNode.InnerText;
            var jobUrl = GetHrefAttributeFromNode(jobTitleNode).Value;

            var companyNameNode = ancorNodes.Single(n => GetHrefAttributeFromNode(n).Value.Contains("/company/"));

            var companyName = companyNameNode.InnerText;
            var companyUrl = GetHrefAttributeFromNode(companyNameNode).Value;

            var locationNode = htmlListing.QuerySelectorAll("td").ElementAt(2);
            var location = locationNode.InnerText;

            var datePostedNode = htmlListing.QuerySelectorAll("td").ElementAt(3);

            var listing = new DiceListing()
            {
                JobTitle = jobTitle,
                JobTitleUrl = jobUrl,
                CompanyName = companyName,
                CompanyNameUrl = companyUrl,
                Location = location,
                LocationUrl = "",
                DatePosted = DateTime.Parse(datePostedNode.InnerText)
            };
            return listing;
        }

        private static HtmlAttribute GetHrefAttributeFromNode(HtmlNode ancorNode)
        {
            return ancorNode.Attributes.Single(attr => attr.Name == "href");
        }
    }
}
