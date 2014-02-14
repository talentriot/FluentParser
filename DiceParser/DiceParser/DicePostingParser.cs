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
    public class DicePostingParser
    {
        public List<DicePosting> GetAllPostings(HtmlDocument DicePage)
        {
            var document = DicePage.DocumentNode;

            var postingTable = document.QueryFromSelectorChain("table.summary");
            var allJobPostings = postingTable.QueryManyFromSelectorChain("tr");

            return allJobPostings.Select(htmlListing => GetPostingFromTableRow(htmlListing)).ToList(); 
        }

        private static DicePosting GetPostingFromTableRow(HtmlNode htmlPosting)
        {
            return null;
        }
    }
}
