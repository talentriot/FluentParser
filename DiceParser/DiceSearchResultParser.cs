using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using HtmlAgilityPack;
using FluentParser;

namespace DiceParser
{
    class DiceSearchResultParser
    {
        public DiceSearchResult GetAllSearchResultData(HtmlDocument htmlDocument)
        {
            var documentRoot = htmlDocument.DocumentNode;
            var refineControlsIdsDict = GetRefineControlsIdsDict(documentRoot);
            var totalSearchResults = GetTotalSearchResults(documentRoot);
            var searchKeywords = GetSearchKeywords(documentRoot, refineControlsIdsDict);
            var zipcodeSearched = GetZipcodeSearched(documentRoot, refineControlsIdsDict);
            return new DiceSearchResult
            {
                TotalSearchResults = totalSearchResults,
                SearchKeywords = searchKeywords,
                ZipcodeSearched = zipcodeSearched
            };
        }

        private int? GetZipcodeSearched(HtmlNode documentRoot, Dictionary<string, List<int>> idsDictionary)
        {
            if (!idsDictionary.ContainsKey("Zipcode"))
            {
                return null;
            }
            var zipcodeId = "Zipcode_" + idsDictionary["Zipcode"];
            var zipcode = documentRoot.QueryFromSelectorChain(zipcodeId, ".undoLabel");
            return Convert.ToInt16(zipcode);
        }

        private static List<string> GetSearchKeywords(HtmlNode documentRoot, Dictionary<string,List<int>> idsDictionary)
        {
            var searchKeywords = new List<string>();
            if (!idsDictionary.ContainsKey("Keyword"))
            {
                return searchKeywords;
            }
            foreach (var idIndex in idsDictionary.Values)
            {
                var keywordId = "Keyword_" + idIndex;
                var keyword = documentRoot.QueryFromSelectorChain(keywordId, ".undoLabel").InnerText;
                searchKeywords.Add(keyword);
            }
            return searchKeywords;
        }

        private static int GetTotalSearchResults(HtmlNode documentRoot)
        {
            var searchResults = documentRoot.QueryFromSelectorChain("#searchResHD", "h2").InnerText;
            var splitString = new string[] {"of"};
            var splitSearchResults = searchResults.Split(splitString, StringSplitOptions.None);
            var totalSearchResults = Convert.ToInt16(splitSearchResults[1]);
            return totalSearchResults;
        }

        private Dictionary<string, List<int>> GetRefineControlsIdsDict(HtmlNode htmlDocument)
        {
            var dictionary = new Dictionary<string, List<int>>();
            var refineControlsContainer = GetRefineControlsDivs(htmlDocument);
            foreach (var htmlNode in refineControlsContainer)
            {
                var splitId = htmlNode.Id.Split('_');
                if (splitId.Length != 2)
                {
                    continue;
                }
                var idKeyword = splitId[0];
                var idIndex = Convert.ToInt16(splitId[1]);
                if (dictionary.ContainsKey(idKeyword))
                {
                    dictionary[idKeyword].Add(idIndex);
                }
                else
                {
                    dictionary[idKeyword] = new List<int> { idIndex };
                }
            }
            return dictionary;
        }

        private static IEnumerable<HtmlNode> GetRefineControlsDivs(HtmlNode documentRoot)
        {
            return documentRoot.QueryManyFromSelectorChain(".refineControls", "div[id]");
        }
    }
}
