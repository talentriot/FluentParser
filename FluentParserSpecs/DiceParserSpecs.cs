using System;
using System.Linq;
using System.Text.RegularExpressions;
using DiceParser;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceParserSpecs
    {
        public static readonly string DiceSearchResultsPage = "./Pages/DiceSearchResultPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Search_Results_Page()
        {
            var searchPageSource = DiceSearchResultsPage.ReadHtmlFromFile();

            var expectedNumberOfListings = GetExpectedSearchResultCount(searchPageSource);

            var parser = new DiceSearchPageParser();
            var parsedListings = parser.GetAllFor(searchPageSource);

            Assert.AreEqual(expectedNumberOfListings, parsedListings.Count());
        }

        private int GetExpectedSearchResultCount(HtmlDocument searchPageSource)
        {
            var searchResultsSection = searchPageSource.DocumentNode.QueryFromSelectorChain("#searchResHD", "h2");
            var searchResultsTest = searchResultsSection.InnerText;

            // Search results: 1 - 30 of 214 => [1,30,214] => 30
            var numberComponents = Regex.Matches(searchResultsTest, @"\d+");
            var starting = numberComponents[0].Value.AsInt();
            var ending = numberComponents[1].Value.AsInt();

            return (ending - starting) + 1;
        }
    }
}
