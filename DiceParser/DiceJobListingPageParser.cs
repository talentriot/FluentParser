using FluentParser;
using HtmlAgilityPack;

namespace DiceParser
{
    public class DiceJobListingPageParser
    {
        //style A
        public DiceJobListing ParseItemsFromPageA(HtmlDocument doc)
        {
            var job = new DiceJobListing();
            var root = doc.DocumentNode;
            job.JobTitle = root.QueryFromSelectorChain("h1#jobTitle").InnerText;
            job.Company = root.QueryFromSelectorChain("div.col-sm-8.col-xs-12", "h2").InnerText;
            job.JobLocation = job.Company; //TODO: string stored in form "for {company} in {Location}".  Parse Dat.
            job.CompanyUrl = root.QueryFromSelectorChain("div.companyLogo","a").GetAttributeValue("href",null);

            return null;
        }

        //style B conglomerate when similarities are known
        public DiceJobListing ParseItemsFromPageB(HtmlDocument doc)
        {
            return null;
        }
    }
}
