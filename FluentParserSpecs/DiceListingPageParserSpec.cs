using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;
using DiceParser;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceListingPageParserSpec
    {

        public static readonly string DiceListingPage = "./Pages/DiceListingPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Listing_Page()
        {
            var htmlDocument = DiceListingPage.ReadHtmlFromFile();
            var expectedNumberOfListings = GetExpectedNumberOfListings(htmlDocument);

            var diceParser = new DiceListingPageParser();
            var listings = diceParser.GetAllListingsFor(htmlDocument);

            Assert.AreEqual(expectedNumberOfListings, listings.Count());
        }

        private int GetExpectedNumberOfListings(HtmlDocument htmlDocument)
        {
            return 28;
        }

    }
}
