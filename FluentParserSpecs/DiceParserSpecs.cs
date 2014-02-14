using System.Linq;
using DiceParser;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    class DiceParserSpecs
    {
        public static readonly string DiceListingPagePath = "./Pages/DiceListingPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Listing_Page()
        {
            var htmlDocument = DiceListingPagePath.ReadHtmlFromFile();

            var parser = new DiceListingPageParser();
            var parsedListings = parser.GetAllFor(htmlDocument).ToList();

            Assert.AreEqual(30, parsedListings.Count());

         
        }
    }
}
