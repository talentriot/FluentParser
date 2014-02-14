using System;
using System.Collections.Generic;

namespace DiceParser
{
    public class DiceJobDetail
    {
        public IEnumerable<string> Skills { get; set; }
        public string Title { get; set; }
        public string AreaCode { get; set; }
        public string TravelRequirement { get; set; }
        public string Telecommute { get; set; }
        public string PayRate { get; set; }
        public string TaxTerm { get; set; }
        public string JobLength { get; set; }
        public int PositionId { get; set; }
        public long DiceId { get; set; }
        public string DetailsHtml { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    public class ContactInfo
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public Uri WebSite { get; set; }
    }
}