using System;
using NUnit.Framework;

namespace FluentParserSpecs
{
    [TestFixture]
    public class DiceParserSpecs
    {
        public static readonly string DiceSearchResultsPage = "./Pages/DiceSearchResultPage.html";

        [Test]
        public void _001_We_Should_Be_Able_To_Parse_A_Search_Results_Page()
        {
            var htmlDocument = DiceSearchResultsPage.ReadHtmlFromFile();
            throw new NotImplementedException();
        }
    }
}
