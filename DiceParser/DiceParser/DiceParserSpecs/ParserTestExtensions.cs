using System.IO;
using HtmlAgilityPack;

namespace DiceParserSpecs
{
    public static class ParserTestExtensions
    {
        public static HtmlDocument ReadHtmlFromFile(this string filePath)
        {
            var htmlSource = File.ReadAllText(filePath).Replace("\n", "");
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlSource);

            return htmlDocument;
        }
    }
}
