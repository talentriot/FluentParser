using System;
using System.Linq;
using DiceParser;
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

            var parser = new DiceSearchPageParser();
            var parsedListings = parser.GetAllFor(searchPageSource);

            var expectedNumberOfListings = 30;
            Assert.AreEqual(expectedNumberOfListings, parsedListings.Count());
        }
    }
}
