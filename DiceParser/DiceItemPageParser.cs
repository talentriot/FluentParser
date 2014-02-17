using System;
using System.Collections.Generic;
using System.Linq;
using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceItemPageParser
    {
        public DiceJobListing ParseItemFromPage(HtmlDocument diceItemPageSource)
        {
            var documentRoot = diceItemPageSource.DocumentNode;

            var schemaOrgJobListingNode = GetJobPostingNodeFrom(documentRoot);

            var spanPropertyContentLookup = GetPropertyLookupDictionaryFrom(schemaOrgJobListingNode);

            var title = spanPropertyContentLookup["title"];
            var industry = spanPropertyContentLookup["industry"];
            var description = spanPropertyContentLookup["description"];

            var datePosted = spanPropertyContentLookup["datePosted"];
            var postedDate = GetPostedDateFor(datePosted);

            var imageUrl = spanPropertyContentLookup["image"];
            var pageUrl = spanPropertyContentLookup["url"];
            var skills = spanPropertyContentLookup["skills"];
            var baseSalary = spanPropertyContentLookup["baseSalary"];
            var employmentType = spanPropertyContentLookup["employmentType"];
            var city = spanPropertyContentLookup["addressLocality"];
            var state = spanPropertyContentLookup["addressRegion"];
            var companyName = spanPropertyContentLookup["organizationOrganization"];
            var companyPageUrl = spanPropertyContentLookup["organizationurl"];

            return new DiceJobListing
            {
                BaseSalary = baseSalary,
                Description = description,
                EmploymentType = employmentType,
                HiringOrganizationName = companyName,
                HiringOrganizationUrl = companyPageUrl,
                ImageUrl = imageUrl,
                Industry = industry,
                LocationCity = city,
                LocationState = state,
                PageUrl = pageUrl,
                PostedDate = postedDate,
                Skills = skills,
                Title = title
            };
        }

        private static Dictionary<string, string> GetPropertyLookupDictionaryFrom(HtmlNode schemaOrgJobListingNode)
        {
            // Flatten out spans so we only see content spans and not 'wrapper' spans.
            var jobListingSpans = schemaOrgJobListingNode.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element);
            var locationSpan = jobListingSpans.First(x => x.Attributes["property"].Value.Equals("jobLocation"));
            var organizationSpan = jobListingSpans.First(x => x.Attributes["property"].Value.Equals("hiringOrganization"));

            var contentSpans = jobListingSpans
                .Where(x => x.Attributes.Contains("content"))
                .Concat(locationSpan.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element));

            // <span property="propName" content="propValue"/> -> Dict[propName -> propValue]
            var spanPropertyContentLookup = contentSpans.ToDictionary(k => k.Attributes["property"].Value,
                v => v.Attributes["content"].Value);
            foreach (var childNode in organizationSpan.ChildNodes)
            {
                // there are duplicate property names, preface them with organization to avoid duplicate key exception.
                if (childNode.NodeType.Equals(HtmlNodeType.Element))
                {
                    spanPropertyContentLookup.Add("organization" + childNode.Attributes["property"].Value,
                        childNode.Attributes["content"].Value);
                }
            }
            return spanPropertyContentLookup;
        }

        private static DateTime GetPostedDateFor(string datePostedString)
        {
            var components = datePostedString.Split('-');
            var month = components[0].AsInt();
            var day = components[1].AsInt();
            var year = components[2].AsInt();

            return new DateTime(year, month, day);
        }

        private static HtmlNode GetJobPostingNodeFrom(HtmlNode documentRoot)
        {
            // Two types of item page, servlet and 'standard', figure out which type it is.

            var doc3Element = documentRoot.TryQuerySelectorChain("div#doc3");

            var htmlElementChildren = doc3Element == null ? 
                documentRoot.QueryFromSelectorChain("html", "body").ChildNodes :
                doc3Element.QueryFromSelectorChain("div.yui-g").ChildNodes;

            return htmlElementChildren.First(x => x.Attributes.Contains("typeof") && x.Attributes["typeof"].Value == "JobPosting");
        }
    }
}
