using System.IO;
using System.Linq;
using DiceParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace ParserSpecs
{
    [TestFixture]
    public class DiceResultListingTests
    {
        private static readonly string DiceSearchResultPage = "./Pages/results.html";
        private static readonly string FifthDiceSearchResultPage = "./Pages/results5.html";

        private string _resultsPageRaw;
        private HtmlDocument _resultPage;
        private HtmlDocument _fifthResultPage;
        private DiceResultListingParser _parser;

        [TestFixtureSetUp]
        public void Init()
        {
            var htmlSource = File.ReadAllText(DiceSearchResultPage).Replace("\n", "");
            var fifthHtmlSource = File.ReadAllText(FifthDiceSearchResultPage).Replace("\n", "");

            _resultPage = new HtmlDocument();
            _resultPage.LoadHtml(htmlSource);

            _resultsPageRaw = htmlSource;

            _fifthResultPage = new HtmlDocument();
            _fifthResultPage.LoadHtml(fifthHtmlSource);

            _parser = new DiceResultListingParser();
        }

        [Test]
        public void _001_Can_Get_Result_Listings()
        {
            var results = _parser.GetAll(_resultPage).ToList();
            var results5 = _parser.GetAll(_fifthResultPage).ToList();

            Assert.AreEqual(30, results.Count);
            Assert.AreEqual(30, results5.Count);
        }

        [Test]
        public void _002_Can_Get_Results_Listing_From_Raw_Html()
        {
            var results = _parser.GetAll(_resultsPageRaw).ToList();

            Assert.AreEqual(30, results.Count);
        }
    }
}