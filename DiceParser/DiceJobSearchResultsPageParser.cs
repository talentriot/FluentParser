using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceJobSearchResultsPageParser
    {
        public IEnumerable<DiceJobPost> GetAllFor(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;

//            var breadcrumbs = documentRoot.QueryManyFromSelectorChain("header.bchead", ".crumb");
//            var categoryText = breadcrumbs.Last().QuerySelector("a").InnerText;
            var categoryText = @"";

            var allHtmlListings = documentRoot.QueryManyFromSelectorChain("div.srSummary", "tbody", "tr.STDsrRes,tr.gold");

            return allHtmlListings.Select(htmlListing => GetListingFromHtmlRow(htmlListing, categoryText));
        }

        private static DiceJobPost GetListingFromHtmlRow(HtmlNode htmlJobPost, string categoryText)
        {
            var titleTd = htmlJobPost.QueryXth(1, "td");
            var titleAnchor = titleTd.QuerySelector("a");
            var titleUrl = titleAnchor.GetAttributeValue("href", null);
            var titleText = titleAnchor.InnerText.Trim();

            var companyTd = htmlJobPost.QueryXth(2, "td");
            var companyAnchor = companyTd.QuerySelector("a");
            var companyText = companyAnchor.InnerText.Trim();

            var locationTd = htmlJobPost.QueryXth(3, "td");
            var locationText = locationTd.InnerText.Trim();

            var datePostedTd = htmlJobPost.QueryXth(4, "td");
            var datePostedText = datePostedTd.InnerText.Trim();
            var datePosted = DateTime.MinValue;
            try
            {
                datePosted = DateTime.ParseExact(datePostedText, @"MMM-dd-yyyy", null);
            }
            catch (FormatException)
            {
                Console.WriteLine(@"Unable to parse posted date: " + datePostedText);
            }

            var jobPost = new DiceJobPost
            {
                PostUrl = titleUrl,
                JobTitle = titleText,
                PostCompany = companyText,
                PostLocation = locationText,
                DatePosted = datePosted
            };
            return jobPost;
        }
    }
}
