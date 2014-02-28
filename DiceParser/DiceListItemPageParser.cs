using System.Collections.Generic;
using System.Linq;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceListItemPageParser
    {
        public List<DiceListItem> ParseItemsFromPage(HtmlDocument diceListPage)
        {
            var itemList = new List<DiceListItem>();
            var root = diceListPage.DocumentNode;
            var searchTable = root.QueryFromSelectorChain("table.summary");
            var standardRows = searchTable.QueryManyFromSelectorChain(".STDsrRes");
            var goldRows = searchTable.QueryManyFromSelectorChain(".gold");

            foreach (var goldRow in goldRows)
            {
                itemList.Add(ParseJobTableRow(goldRow));
            }

            foreach (var standardRow in standardRows)
            {
                itemList.Add(ParseJobTableRow(standardRow));
            }

            return itemList;
        }

        private DiceListItem ParseJobTableRow(HtmlNode tableRow)
        {
            var item = new DiceListItem();
            var rowColumns = tableRow.QueryManyFromSelectorChain("td").ToList();

            var jobDiv = rowColumns[0].QueryFromSelectorChain("div");
            item.JobUrl = jobDiv.FirstChild.GetAttributeValue("href",null);
            item.JobTitle = jobDiv.InnerText.Trim();

            var companyDiv = rowColumns[1];
            item.CompanyUrl = companyDiv.QueryFromSelectorChain("a").GetAttributeValue("href", null);
            item.Company = companyDiv.InnerText.Trim();

            item.Location = rowColumns[2].InnerText.Trim();
            var locationUrlNode = rowColumns[2].TryQuerySelectorChain("a");
            if (locationUrlNode != null)
            {
                item.LocationUrl = locationUrlNode.GetAttributeValue("href", null);   
            }
            item.DatePosted = rowColumns[3].InnerText.Trim();

            return item;
        }
    }
}
