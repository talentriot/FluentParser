using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceParser
{
    public class DicePosting
    {
        public string Id { get; set; }
        public DateTime DatePosted { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string PostingUrl { get; set; }
    }
}
