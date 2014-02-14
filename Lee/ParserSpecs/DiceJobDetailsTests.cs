using System.IO;
using System.Linq;
using DiceParser;
using HtmlAgilityPack;
using NUnit.Framework;

namespace ParserSpecs
{
    [TestFixture]
    public class DiceJobDetailsTests
    {
        private static readonly string JobDetailsPage = "./Pages/jobdetails.html";
        private HtmlDocument _detailsPage;
        private DiceJobDetailsParser _detailsParser;

         [TestFixtureSetUp]
         public void Init()
         {
             var detailsHtml = File.ReadAllText(JobDetailsPage).Replace("\n", "");
             _detailsPage = new HtmlDocument();
             _detailsPage.LoadHtml(detailsHtml);

             _detailsParser = new DiceJobDetailsParser();
         }

        [Test]
        public void _001_Should_Get_Job_Details()
        {
            var jobDetails = _detailsParser.GetDetails(_detailsPage);

            Assert.AreEqual(10, jobDetails.Skills.Count());
        }
    }
}