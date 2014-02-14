using System.Linq;
using System.Text.RegularExpressions;
using DiceParser;
using HtmlAgilityPack;
using NUnit.Framework;
using FluentParser;

namespace DiceParserSpecs
{
    [TestFixture]
    class DiceParserSpecs
    {
        public static readonly string DiceNETJobSearchPagePath = "./Pages/DiceNetJobSearchPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Dice_Job_Listing_Page()
        {
            var htmlDocument = DiceNETJobSearchPagePath.ReadHtmlFromFile();

            var parser = new DiceJobListingPageParser();

            var allListings = parser.GetAllFor(htmlDocument).ToList();

            var expectedListings = GetExpectedNumberOfListings(htmlDocument);

            Assert.AreEqual(allListings.Count, expectedListings);
        }

        private static int GetExpectedNumberOfListings(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var searchResultsSection = documentRoot.QueryFromSelectorChain("#searchResHDcontainer", "#searchResHD", "h2");

            var searchResultsString = searchResultsSection.InnerText;

            var searchNumbers = Regex.Matches(searchResultsString, @"\d+");

            var numberOfListings = searchNumbers[1].Value.AsInt();

            return numberOfListings;
        }
    }
}
