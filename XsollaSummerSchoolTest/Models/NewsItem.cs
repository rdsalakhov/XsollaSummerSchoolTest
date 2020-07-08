using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XsollaSummerSchoolTest.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        public string Headline { get; set; }

        public string Body { get; set; }

        public int RateSum { get; set; }

        public int RateCount { get; set; }
    }
}