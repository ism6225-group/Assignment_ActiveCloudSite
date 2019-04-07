using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment_ActiveCloudSite.Models;

namespace Assignment_ActiveCloudSite.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
    }
}
