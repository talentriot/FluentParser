using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace FluentParser
{
    public static class ParserExtensions
    {
        public static HtmlNode QueryXth(this HtmlNode node, int number, params string[] selectorChain)
        {
            var nodes = node.QueryManyFromSelectorChain(selectorChain).ToList();
            var nodeCount = nodes.Count();
            if (nodeCount < number)
            {
                throw new ParseException(string.Format("Cannot return item {0} because there are only {1}!", number,
                    nodeCount));
            }
            return nodes.ElementAt(number - 1);
        }

        public static IEnumerable<HtmlNode> QueryManyFromSelectorChain(this HtmlNode node, params string[] selectorChain)
        {
            var lastSelector = selectorChain.Last();
            if (selectorChain.Length > 1)
            {
                selectorChain = selectorChain.Take(selectorChain.Length - 1).ToArray();
                node = node.QueryFromSelectorChain(selectorChain);
            }

            var nodes = node.QuerySelectorAll(lastSelector);
            if (nodes == null)
            {
                var errorMessage = GetChainErrorMessage(selectorChain.Take(selectorChain.Length - 1), lastSelector);
                throw new ParseException(errorMessage);
            }

            return nodes;
        }

        public static HtmlNode QueryFromSelectorChain(this HtmlNode node, params string[] selectorChain)
        {
            var foundLinks = new List<string>();
            foreach (var selector in selectorChain)
            {
                node = node.QuerySelector(selector);
                if (node == null)
                {
                    var errorMessage = GetChainErrorMessage(foundLinks, selector);
                    throw new ParseException(errorMessage);
                }
                foundLinks.Add(selector);
            }

            return node;
        }

        public static HtmlNode TryQuerySelectorChain(this HtmlNode node, params string[] selectorChain)
        {
            foreach (var selector in selectorChain)
            {
                node = node.QuerySelector(selector);
                if (node == null)
                {
                    return null;
                }
            }

            return node;
        }

        private static string GetChainErrorMessage(IEnumerable<string> foundLinks, string missingLink)
        {
            var errorBuilder = new StringBuilder();
            errorBuilder.Append(string.Format("Found {0}", string.Join(" > ", foundLinks)));
            errorBuilder.Append(" but could not find: " + missingLink);
            return errorBuilder.ToString();
        }

        public static int AsInt(this string input)
        {
            return int.Parse(input);
        }

        public static int ShortMonthStringToInt(this string input)
        {
            switch (input.ToLower())
            {
                case "jan":
                    return 1;
                case "feb":
                    return 2;
                case "mar":
                    return 3;
                case "apr":
                    return 4;
                case "may":
                    return 5;
                case "jun":
                    return 6;
                case "jul":
                    return 7;
                case "aug":
                    return 8;
                case "sep":
                    return 9;
                case "oct":
                    return 10;
                case "nov":
                    return 11;
                case "dec":
                    return 12;
            }
            
            throw new ArgumentException(string.Format("Invalid short month string: {0}, must be the first three digits of a month.", input));
        }
    }

    public class ParseException : Exception
    {
        public ParseException(string errorMessage) : base(errorMessage)
        {   
        }
    }
}
