using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceParser
{
    class DiceSearchResult
    {
        public int TotalSearchResults { get; set; }
        public List<string> SearchKeywords { get; set; }
        public int? ZipcodeSearched { get; set; }
        public string CitySearched { get; set; }
        public string StateSearched { get; set; }
        public int SearchRadiusInMiles { get; set; }
        public IEnumerable<DiceListing> Listings { get; set; }
    }
}
