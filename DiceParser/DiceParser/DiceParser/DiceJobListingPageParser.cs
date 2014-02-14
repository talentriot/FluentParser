using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using FluentParser;

namespace DiceParser
{
    public class DiceJobListingPageParser
    {
        public IEnumerable<DiceJobListing> GetAllFor(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var allNormalListingRows = documentRoot.QueryManyFromSelectorChain("#SRmCol", ".srSummary", ".STDsrRes");
            var allGoldListingRows = documentRoot.QueryManyFromSelectorChain("#SRmCol", ".srSummary", ".gold");

            var allListings = allNormalListingRows.ToList().Union(allGoldListingRows);

            var diceJobListings = new List<DiceJobListing>();

            foreach (var listing in allListings)
            {
                diceJobListings.Add(GetListingFromHtmlRow(listing));
            }

            return diceJobListings;
        }

        public DiceJobListing GetListingFromHtmlRow(HtmlNode htmlListing)
        {
            var cells = htmlListing.QueryManyFromSelectorChain("td").ToList();

            var titleCell = cells[0];
            var companyCell = cells[1];
            var locationCell = cells[2];
            var datePostedCell = cells[3];

            var title = titleCell.ChildNodes[1].ChildNodes[0].InnerHtml;
            var company = companyCell.ChildNodes[1].InnerHtml;
            string location;

            if (locationCell.ChildNodes.Count > 1)
            {
                location = locationCell.ChildNodes[1].InnerHtml;
            }
            else
            {
                location = locationCell.ChildNodes[0].InnerHtml.Trim(Convert.ToChar(" "));
            }
            
            var datePosted = datePostedCell.InnerHtml;

            var diceJobListing = new DiceJobListing
                {
                    JobTitle = title,
                    CompanyName = company,
                    Location = location,
                    DatePosted = DateTime.Parse(datePosted)
                };

            return diceJobListing;
        }
    }
}
