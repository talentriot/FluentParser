using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentParser;
using HtmlAgilityPack;
using NUnit.Framework;
using DiceParser;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceParserSpecs
    {
        public static readonly string DicePostingsPagePath = "./Pages/java.html";

        [Test]
        public void _001_We_should_be_able_to_parse_a_postings_page()
        {
            var htmlDocument = DicePostingsPagePath.ReadHtmlFromFile();

            var parser = new DicePostingParser();
            var parsedPostings = parser.GetAllPostings(htmlDocument);
        }

        private void GetExpectedNumberOfPostings()
        {
        }
    }
}
