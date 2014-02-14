using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiceParser;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceParserSpects
    {
        public static readonly string DicePostSearchResultsPagePath = "../../Pages/DiceJobSearchResultsPage.html";
        public static readonly string DicePostPagePath = "../../Pages/DiceJobPostPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Search_Results_Page()
        {
            var htmlDocument = DicePostSearchResultsPagePath.ReadHtmlFromFile();
            var expectedNumberOfListings = GetExpectedNumberOfListings(htmlDocument);

            var parser = new DiceJobSearchResultsPageParser();
            var parsedPosts = parser.GetAllFor(htmlDocument).ToList();

            Assert.AreEqual(expectedNumberOfListings, parsedPosts.Count());

            Parallel.ForEach(parsedPosts, new ParallelOptions {MaxDegreeOfParallelism = 1}, AssertIsValidJobPostPageItem);
        }

        private static void AssertIsValidJobPostPageItem(DiceJobPost jobPost)
        {
            Assert.IsNotNullOrEmpty(jobPost.JobTitle);
            Assert.IsNotNullOrEmpty(jobPost.PostUrl);
            Assert.IsNotNullOrEmpty(jobPost.PostCompany);
            Assert.IsNotNullOrEmpty(jobPost.PostLocation);
            Assert.AreNotEqual(jobPost.DatePosted, DateTime.MinValue);
        }

        [Test]
        public void _002_We_should_be_able_to_parse_a_job_post_page()
        {
            var htmlDocument = DicePostPagePath.ReadHtmlFromFile();

            var parser = new DiceJobPostPageParser();
            var parsedItem = parser.ParseJobPostDetailsFromPage(htmlDocument);
//            AssertItemParsedCorrectly(parsedItem, false);
//
//            parsedItem = parser.ParseItemFromPage(mapDocument);
//            AssertItemParsedCorrectly(parsedItem, true);
        }

//        private void AssertItemParsedCorrectly(CraigslistItem parsedItem, bool shouldHaveMapLinks)
//        {
//            if (shouldHaveMapLinks)
//            {
//                Assert.IsTrue(parsedItem.HasMapLocation);
//                Assert.IsNotNullOrEmpty(parsedItem.Address);
//                Assert.IsNotNullOrEmpty(parsedItem.GoogleMapsUrl);
//                Assert.IsNotNullOrEmpty(parsedItem.YahooMapsUrl);
//            }
//            else
//            {
//                Assert.IsFalse(parsedItem.HasMapLocation);
//            }
//
//            Assert.IsNotNullOrEmpty(parsedItem.CraigslistCategoryText);
//            Assert.IsNotNull(parsedItem.HasImage);
//            if (parsedItem.HasImage)
//            {
//                Assert.IsTrue(parsedItem.HasImage);
//                Assert.IsNotNullOrEmpty(parsedItem.ImageUrl);
//            }
//            else
//            {
//                Assert.IsFalse(parsedItem.HasImage);
//            }
//            Assert.IsNotNullOrEmpty(parsedItem.Id);
//            Assert.IsNotNullOrEmpty(parsedItem.ItemPageUrl);
//            Assert.IsNotNullOrEmpty(parsedItem.ListingTitle);
//            Assert.IsNotNullOrEmpty(parsedItem.LocationRegion);
//            Assert.IsNotNullOrEmpty(parsedItem.PostingBody);
//
//            Assert.IsNotNull(parsedItem.LastUpdated);
//            Assert.True(parsedItem.LastUpdated.Year >= 2014);
//            Assert.IsNotNull(parsedItem.PostingDate);
//        }

        private static int GetExpectedNumberOfListings(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var itemNumbersSection = documentRoot.QueryFromSelectorChain("#searchResHDcontainer", "#searchResHD", "h2").InnerText;

            var recordNumbers = Regex.Matches(itemNumbersSection, @"\d+");
            var starting = recordNumbers[0].Value.AsInt();
            var ending = recordNumbers[1].Value.AsInt();

            var expectedNumberOfListings = ending - starting + 1;
            return expectedNumberOfListings;
        }
    }
}
