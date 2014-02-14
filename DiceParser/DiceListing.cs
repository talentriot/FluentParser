using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiceParser
{
    public class DiceListing
    {
        public string Id { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameUrl { get; set; }
        public string Location { get; set; }
        public string LocationUrl { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
