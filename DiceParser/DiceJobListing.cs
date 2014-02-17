using System;

namespace DiceParser
{
    public class DiceJobListing
    {
        public string Title { get; set; }
        public string PageUrl { get; set; }
        public DateTime PostedDate { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Skills { get; set; }
        public string BaseSalary { get; set; }
        public string EmploymentType { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string HiringOrganizationName { get; set; }
        public string HiringOrganizationUrl { get; set; }
    }
}