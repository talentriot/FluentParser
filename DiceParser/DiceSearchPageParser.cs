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

        private DiceSearchResult GetDiceSearchResultFromNode(HtmlNode searchRow)
        {
            return new DiceSearchResult();
        }
    }
}
