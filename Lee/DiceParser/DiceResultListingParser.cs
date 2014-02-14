using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceResultListingParser
    {
        private static readonly int JobTitleColumnIndex = 0;
        private static readonly int CompanyColumnIndex = 1;
        private static readonly int LocationColumnIndex = 2;
        private static readonly int DatePostedColumnIndex = 3;

        public IEnumerable<DiceResultListing> GetAll(HtmlDocument document)
        {
            var documentRoot = document.DocumentNode;
            var standardResultRows = documentRoot.QuerySelectorAll("tr.STDsrRes");
            var goldResultRows = documentRoot.QuerySelectorAll("tr.gold");

            return standardResultRows.Concat(goldResultRows).Select(GetResultListing).ToList();
        }

        public IEnumerable<DiceResultListing> GetAll(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return GetAll(document);
        }

        private DiceResultListing GetResultListing(HtmlNode resultNode)
        {
            var resultColumns = resultNode.QuerySelectorAll("td").ToList();

            var jobTitle = GetJobTitle(resultColumns);
            var jobUrl = GetJobUrl(resultColumns);
            
            var companyName = GetCompanyName(resultColumns);

            var location = GetLocation(resultColumns);

            var postedDate = GetPostedDate(resultColumns);

            return new DiceResultListing
                {
                    Company = companyName,
                    DetailsPage = jobUrl,
                    JobLocation = location,
                    JobTitle = jobTitle,
                    PostedDate = postedDate
                };
        }

        private static DateTime? GetPostedDate(IEnumerable<HtmlNode> resultColumns)
        {
            var postedDateString = resultColumns.ElementAt(DatePostedColumnIndex).InnerHtml;

            DateTime postedDate;
            if (!DateTime.TryParse(postedDateString, out postedDate))
            {
                return null;
            }

            return postedDate;
        }

        private static string GetCompanyName(IEnumerable<HtmlNode> resultColumns)
        {
            var companyInfo = resultColumns.ElementAt(CompanyColumnIndex);
            var companyLink = companyInfo.QuerySelector("a");
            return companyLink == null ? companyInfo.InnerHtml.Replace("\n", "").Trim() : companyLink.InnerText;
        }

        private string GetJobTitle(IEnumerable<HtmlNode> resultColumns)
        {
            var jobInfo = GetJobNode(resultColumns);
            return jobInfo.InnerText;
        }

        private Uri GetJobUrl(IEnumerable<HtmlNode> resultColumns)
        {
            var jobInfo = GetJobNode(resultColumns);
            var relativeUrl = jobInfo.Attributes["href"].Value;
            return new Uri(relativeUrl, UriKind.Relative);
        }

        private HtmlNode GetJobNode(IEnumerable<HtmlNode> resultColumns)
        {
            return resultColumns.ElementAt(JobTitleColumnIndex).QuerySelector("a");
        }

        private string GetLocation(IEnumerable<HtmlNode> resultColumns)
        {
            var locationInfo = resultColumns.ElementAt(LocationColumnIndex);
            var locationInnerNode = locationInfo.QuerySelector("a");
            return locationInnerNode == null ? locationInfo.InnerText.Replace("\n", "").Trim() : locationInnerNode.InnerText;
        }

        
    }
}