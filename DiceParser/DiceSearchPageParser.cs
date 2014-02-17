using System;
using System.Collections.Generic;
using System.Linq;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceSearchPageParser
    {
        public IEnumerable<DiceSearchResult> GetAllFor(HtmlDocument searchPageSource)
        {
            var rootNode = searchPageSource.DocumentNode;

            var rowContainer = rootNode.QueryFromSelectorChain("div#SRmCol", "div.srSummary", "tbody");
            var normalSearchResult = rowContainer.QueryManyFromSelectorChain("tr.STDsrRes");
            var sponsoredRows = rowContainer.QueryManyFromSelectorChain("tr.gold");

            var searchResults = normalSearchResult.Concat(sponsoredRows);

            return searchResults.Select(GetDiceSearchResultFromNode);
        }

        private static DiceSearchResult GetDiceSearchResultFromNode(HtmlNode searchRow)
        {
            var allTds = searchRow.QueryManyFromSelectorChain("td").ToList();
            
            var titleLink = allTds.ElementAt(0).QueryFromSelectorChain("a");
            var title = titleLink.InnerText;
            var relativeUrl = titleLink.GetAttributeValue("href", null);
            var url = String.Format("http://www.dice.com{0}", relativeUrl);

            var company = allTds.ElementAt(1).InnerText;
            var location = allTds.ElementAt(2).InnerText;
            var parsedDate = allTds.ElementAt(3).InnerText;

            var dateComponents = parsedDate.Split('-');
            var month = dateComponents[0].ShortMonthStringToInt();
            var day = dateComponents[1].AsInt();
            var year = dateComponents[2].AsInt();

            var date = new DateTime(year, month, day);

            return new DiceSearchResult
            {
                CompanyName = company,
                ItemPageUrl = url,
                Location = location,
                PostingDate = date,
                Title = title
            };
        }
    }
}
