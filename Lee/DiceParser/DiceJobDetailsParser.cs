using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceJobDetailsParser
    {
        private static readonly string BaseUrl = "http://www.dice.com";

        public DiceJobDetail GetDetails(HtmlDocument detailsPage)
        {
            var document = detailsPage.DocumentNode;
            var skills = document.QuerySelectorAll("meta");

            return new DiceJobDetail();
        }
    }
}