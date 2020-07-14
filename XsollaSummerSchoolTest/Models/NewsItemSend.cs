using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XsollaSummerSchoolTest.Models
{
    public class NewsItemSend
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Body { get; set; }
        public string Category { get; set; }
        public int TotalRate { get; set; }
        public int RateCount { get; set; }

        public static implicit operator NewsItemSend(NewsItem newsItem)
        {
            var totalRate = newsItem.Rate.Select(x => int.Parse(x.Mark.ToString())).Sum();
            var rateCount = newsItem.Rate.Count();
            NewsItemSend newsItemSend = new NewsItemSend
            {
                Id = newsItem.Id,
                Headline = newsItem.Headline,
                Body = newsItem.Body,
                Category = newsItem.Category,
                TotalRate = totalRate,
                RateCount = rateCount
            };

            return newsItemSend;
        }
    }
}