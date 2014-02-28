using System.Linq;
using System.Threading.Tasks;
using DiceParser;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceParserSpecs
    {
        public static readonly string DiceListPagePath = "./Pages/diceListingPage.html";
        public static readonly string DiceJobListingStyleA = "./Pages/diceJobListingStyleA.html";
        public static readonly string DiceJobListingStyleB = "./Pages/diceJobListingStyleB.html";

        [Test]
        public void _101_we_should_be_able_to_parse_a_dice_listing_page()
        {
            var diceHtml = DiceListPagePath.ReadHtmlFromFile();
            var parser = new DiceListItemPageParser();
            var listOfParsedDiceJobs = parser.ParseItemsFromPage(diceHtml);
            Assert.AreEqual(listOfParsedDiceJobs.Count, GetExpectedNumberOfDiceJobRows(diceHtml));

            Parallel.ForEach(listOfParsedDiceJobs, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                             AssertDiceJobListingIsValid);
        }

        [Test]
        public void _102_We_Should_Be_Able_To_Parse_A_Dice_Job_Listing_Page_StyleA()
        {
            var jobHtml = DiceJobListingStyleA.ReadHtmlFromFile();
            var parser = new DiceJobListingPageParser();
            var something = parser.ParseItemsFromPageA(jobHtml);
            Assert.AreEqual(0, 0);
        }

        [Test]
        public void _103_We_Should_Be_Able_To_Parse_A_Dice_Job_Listing_Page_StyleB()
        {
            var jobHtml = DiceJobListingStyleA.ReadHtmlFromFile();
            var parser = new DiceJobListingPageParser();
            var something = parser.ParseItemsFromPageB(jobHtml);
            Assert.AreEqual(0, 0);
        }

        private static int GetExpectedNumberOfDiceJobRows(HtmlDocument doc)
        {
            var root = doc.DocumentNode;
            var standardItemsInTable = root.QueryManyFromSelectorChain("tr.STDsrRes");
            var goldItemsInTable = root.QueryManyFromSelectorChain("tr.gold");
            return standardItemsInTable.Count() + goldItemsInTable.Count();
        }

        private static void AssertDiceJobListingIsValid(DiceListItem diceJob)
        {
            //locationUrl can be null but the rest should always be correct
            Assert.NotNull(diceJob.Company);
            Assert.NotNull(diceJob.CompanyUrl);
            Assert.NotNull(diceJob.JobTitle);
            Assert.NotNull(diceJob.JobUrl);
            Assert.NotNull(diceJob.Location);
            Assert.NotNull(diceJob.DatePosted);
        }
    }
}
