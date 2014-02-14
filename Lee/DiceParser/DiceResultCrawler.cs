using System.Collections.Generic;
using System.Net;
using System.Threading;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceResultCrawler
    {
        private static readonly string BaseUrlFormat =
            "http://www.dice.com/job/results/{0}?b=8&caller=basic&q={1}&x=all&p=z&n={2}";

        private static readonly string ResultOffsetFormat = "&o={0}";

        private readonly string _url;
        private readonly int _resultsPerPage;
        private readonly DiceResultListingParser _resultListingParser;
        private readonly WebClient _client;

        public DiceResultCrawler(int zipCode, string searchTerms, int resultsPerPage = 50)
        {
            _url = string.Format(BaseUrlFormat, zipCode, searchTerms, resultsPerPage);
            _resultsPerPage = resultsPerPage;
            _client = new WebClient();
            _resultListingParser = new DiceResultListingParser();
        }

        public DiceResults GetResultsFromPage(int pageNumber)
        {
            var pageUrl = _url;
            var resultOffset = pageNumber*_resultsPerPage;
            if (resultOffset > 0)
            {
                pageUrl += string.Format(ResultOffsetFormat, resultOffset);
            }
            var resultsHtml = _client.DownloadString(pageUrl);

            var document = GetHtmlDocument(resultsHtml);

            var results = _resultListingParser.GetAll(document);
            var hasNextPage = HasNextPage(document);

            return new DiceResults {Listings = results, HasNextPage = hasNextPage};
        }

        public IEnumerable<DiceResultListing> GetAllResults()
        {
            var currentPage = 0;
            var results = new List<DiceResultListing>();

            var currentResult = GetResultsFromPage(currentPage++);
            results.AddRange(currentResult.Listings);
            while (currentResult.HasNextPage)
            {
                // throttle to save poor Dice's precious bandwidth :(
                Thread.Sleep(1200);
                currentResult = GetResultsFromPage(currentPage++);
                results.AddRange(currentResult.Listings);
            }

            return results;
        }

        private static bool HasNextPage(HtmlDocument document)
        {
            var documentRoot = document.DocumentNode;
            var nextPageElement = documentRoot.QuerySelector(".pageProg .nextPage");
            return nextPageElement != null;
        }

        private static HtmlDocument GetHtmlDocument(string rawHtml)
        {
            var document = new HtmlDocument();
            document.LoadHtml(rawHtml);
            return document;
        }

    }
}