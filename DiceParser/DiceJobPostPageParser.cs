using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceJobPostPageParser
    {
        public DiceJobDetailedPost ParseJobPostDetailsFromPage(HtmlDocument htmlJobPostDetails)
        {
            var documentRoot = htmlJobPostDetails.DocumentNode;

            var titleTd = documentRoot.QuerySelector("#jobTitle");
            var titleText = titleTd.InnerText;

            var sideColumnDiv = documentRoot.QuerySelector("div.side-column");

            var paneDivs = sideColumnDiv.QuerySelectorAll("div.paneBtb,div.paneBt");

            var paneDetails = new Dictionary<string, string>();
            foreach (var paneDiv in paneDivs)
            {
                var paneDls = paneDiv.QuerySelectorAll("dl");

                foreach (var paneDl in paneDls)
                {
                    var label = paneDl.QuerySelector("dt").InnerText.Replace(":", string.Empty);
                    var value = paneDl.QuerySelector("dd").InnerText.Replace("&nbsp;", string.Empty);
                    paneDetails.Add(label, value);
                }
            }

            var jobPost = new DiceJobDetailedPost
            {
                PostUrl = "",
                JobTitle = titleText,
                PostCompany = "",
                PostLocation = "",
                DatePosted = new DateTime(),
                skills = paneDetails["Skills"],
            };
            return jobPost;
        }
    }
}
