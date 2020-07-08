using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace XsollaSummerSchoolTest.Models
{
    public class NewsDataContext : DbContext
    {
        public DbSet<NewsItem> NewsItemSet { get; set; }

        public NewsDataContext() : base("NewsDataModelContainer")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<NewsDataContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}