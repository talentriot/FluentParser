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
        public static readonly string DiceStandardItemPage = "./Pages/DiceStandardItemPage.html";
        public static readonly string DiceServletItemPage = "./Pages/DiceServletItemPage.html";

        [Test]
        public void _001_We_Should_Parse_The_Correct_Number_of_Results_From_A_Search_Page()
        {
            var searchPageSource = DiceSearchResultsPage.ReadHtmlFromFile();

            var expectedNumberOfListings = GetExpectedSearchResultCount(searchPageSource);

            var parser = new DiceSearchPageParser();
            var parsedListings = parser.GetAllFor(searchPageSource);

            Assert.AreEqual(expectedNumberOfListings, parsedListings.Count());
        }

        [Test]
        public void _002_Each_Search_Result_Row_Should_Parse_Correctly()
        {
            var searchPageSource = DiceSearchResultsPage.ReadHtmlFromFile();

            var parser = new DiceSearchPageParser();
            var parsedListings = parser.GetAllFor(searchPageSource);

            foreach (var diceSearchResult in parsedListings)
            {
                AssertSearchResultHasExpectedValues(diceSearchResult);
            }
        }

        [Test]
        public void _010_We_should_Parse_A_Standard_dice_Item_Page()
        {
            var parser = new DiceItemPageParser();

            var standardItemPageSource = DiceStandardItemPage.ReadHtmlFromFile();
            var jobListing = parser.ParseItemFromPage(standardItemPageSource);

            AssertJobListingHasExpectedValues(jobListing);
        }

        [Test]
        public void _012_We_should_Parse_A_Dice_Servlet_Item_Page()
        {
            var parser = new DiceItemPageParser();

            var servletItemPageSource = DiceServletItemPage.ReadHtmlFromFile();
            var jobListing = parser.ParseItemFromPage(servletItemPageSource);

            AssertJobListingHasExpectedValues(jobListing);
        }

        private static void AssertSearchResultHasExpectedValues(DiceSearchResult diceSearchResult)
        {
            Assert.IsNotNullOrEmpty(diceSearchResult.Title);
            Assert.IsNotNullOrEmpty(diceSearchResult.CompanyName);
            Assert.IsNotNullOrEmpty(diceSearchResult.Location);
            Assert.IsNotNull(diceSearchResult.PostingDate);
            Assert.IsNotNullOrEmpty(diceSearchResult.ItemPageUrl);
        }

        private static void AssertJobListingHasExpectedValues(DiceJobListing jobListing)
        {
            Assert.IsNotNullOrEmpty(jobListing.Title);
            Assert.IsNotNullOrEmpty(jobListing.PageUrl);
            Assert.IsNotNull(jobListing.PostedDate);
            Assert.IsNotNullOrEmpty(jobListing.Industry);
            Assert.IsNotNullOrEmpty(jobListing.Description);
            Assert.IsNotNullOrEmpty(jobListing.ImageUrl);
            Assert.IsNotNullOrEmpty(jobListing.Skills);
            Assert.IsNotNullOrEmpty(jobListing.BaseSalary);
            Assert.IsNotNullOrEmpty(jobListing.EmploymentType);
            Assert.IsNotNullOrEmpty(jobListing.LocationCity);
            Assert.IsNotNullOrEmpty(jobListing.LocationState);
            Assert.IsNotNullOrEmpty(jobListing.HiringOrganizationName);
            Assert.IsNotNullOrEmpty(jobListing.HiringOrganizationUrl);
        }

        private static int GetExpectedSearchResultCount(HtmlDocument searchPageSource)
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
