using System.Linq;
using DiceParser;
using NUnit.Framework;

namespace ParserSpecs
{
    [TestFixture]
    public class DiceResultCrawlerTests
    {
        private DiceResultCrawler _crawler;

         [TestFixtureSetUp]
         public void Init()
         {
            _crawler = new DiceResultCrawler(80301, ".net");     
         }

        [Test]
        public void _001_Should_Get_First_Page_Results()
        {
            var firstPageResults = _crawler.GetResultsFromPage(1).Listings.ToList();

            Assert.Greater(firstPageResults.Count, 0);
        }

        [Test]
        public void _002_Should_Get_All_Results()
        {
            var results = _crawler.GetAllResults().ToList();

            Assert.Greater(results.Count, 0);
        }

    }
}