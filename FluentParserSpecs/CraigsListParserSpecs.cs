using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CraigslistParser;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    public class CraigsListParserSpecs
    {
        public static readonly string CraigslistItemPageNoMapPath = "./Pages/CraigslistItemPageNoMap.html";
        public static readonly string CraigslistItemPageWithMapPath = "./Pages/CraigslistItemPageWithMap.html";
        public static readonly string CraigslistListingPagePath = "./Pages/CraigslistListingPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Listing_Page()
        {
            var htmlDocument = CraigslistListingPagePath.ReadHtmlFromFile();
            var expectedNumberOfListings = GetExpectedNumberOfListings(htmlDocument);

            var parser = new CraigslistListingPageParser();
            var parsedListings = parser.GetAllFor(htmlDocument).ToList();

            Assert.AreEqual(expectedNumberOfListings, parsedListings.Count());

            Parallel.ForEach(parsedListings, new ParallelOptions {MaxDegreeOfParallelism = 1}, AssertIsValidListingPageItem);

            var missingLatitudes = parsedListings.Count(x => x.LocationLatitude == null);
            var missingLongitudes = parsedListings.Count(x => x.LocationLongitude == null);
            var missingLocationRegions = parsedListings.Count(x => string.IsNullOrWhiteSpace(x.LocationRegion));
            Assert.AreEqual(missingLatitudes, missingLongitudes);
            Console.WriteLine("{0} results had null latitudes.", missingLatitudes);
            Console.WriteLine("{0} results had null longitudes.", missingLongitudes);
            Console.WriteLine("{0} results had null location regions.", missingLocationRegions);
            Console.WriteLine("{0} results had null posting dates.", parsedListings.Count(x => x.PostingDate == null));
        }

        private static void AssertIsValidListingPageItem(CraigslistListing listing)
        {
            Assert.IsNotNullOrEmpty(listing.CraigslistCategoryText);
            Assert.IsNotNull(listing.HasImage);
            if (listing.HasImage)
            {
                Assert.IsTrue(listing.HasImage);
                Assert.IsNotNullOrEmpty(listing.ImageUrl);
            }
            else
            {
                Assert.IsFalse(listing.HasImage);
            }
            Assert.IsNotNull(listing.HasMapLocation);
            Assert.IsNotNull(listing.Id);
            Assert.IsNotNullOrEmpty(listing.Id);
            Assert.IsNotNullOrEmpty(listing.ItemPageUrl);
            Assert.IsNotNullOrEmpty(listing.ListingTitle);
        }

        [Test]
        public void _002_We_should_be_able_to_parse_an_item_page()
        {
            var noMapDocument = CraigslistItemPageNoMapPath.ReadHtmlFromFile();
            var mapDocument = CraigslistItemPageWithMapPath.ReadHtmlFromFile();

            var parser = new CraigslistItemPageParser();
            var parsedItem = parser.ParseItemFromPage(noMapDocument);
            AssertItemParsedCorrectly(parsedItem, false);

            parsedItem = parser.ParseItemFromPage(mapDocument);
            AssertItemParsedCorrectly(parsedItem, true);
        }

        private void AssertItemParsedCorrectly(CraigslistItem parsedItem, bool shouldHaveMapLinks)
        {
            if (shouldHaveMapLinks)
            {
                Assert.IsTrue(parsedItem.HasMapLocation);
                Assert.IsNotNullOrEmpty(parsedItem.Address);
                Assert.IsNotNullOrEmpty(parsedItem.GoogleMapsUrl);
                Assert.IsNotNullOrEmpty(parsedItem.YahooMapsUrl);
            }
            else
            {
                Assert.IsFalse(parsedItem.HasMapLocation);
            }

            Assert.IsNotNullOrEmpty(parsedItem.CraigslistCategoryText);
            Assert.IsNotNull(parsedItem.HasImage);
            if (parsedItem.HasImage)
            {
                Assert.IsTrue(parsedItem.HasImage);
                Assert.IsNotNullOrEmpty(parsedItem.ImageUrl);
            }
            else
            {
                Assert.IsFalse(parsedItem.HasImage);
            }
            Assert.IsNotNullOrEmpty(parsedItem.Id);
            Assert.IsNotNullOrEmpty(parsedItem.ItemPageUrl);
            Assert.IsNotNullOrEmpty(parsedItem.ListingTitle);
            Assert.IsNotNullOrEmpty(parsedItem.LocationRegion);
            Assert.IsNotNullOrEmpty(parsedItem.PostingBody);

            Assert.IsNotNull(parsedItem.LastUpdated);
            Assert.True(parsedItem.LastUpdated.Year >= 2014);
            Assert.IsNotNull(parsedItem.PostingDate);
        }

        private static int GetExpectedNumberOfListings(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

            var itemNumbersSection = documentRoot.QueryFromSelectorChain("#toc_rows", "span.paginator", ".pagenum").InnerText;

            var recordNumbers = Regex.Matches(itemNumbersSection, @"\d+");
            var starting = recordNumbers[0].Value.AsInt();
            var ending = recordNumbers[1].Value.AsInt();

            var expectedNumberOfListings = ending - starting + 1;
            return expectedNumberOfListings;
        }
    }
}
