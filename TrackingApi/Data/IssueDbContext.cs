using Microsoft.EntityFrameworkCore;
using TrackingApi.Models;

namespace TrackingApi.Data
{
    public class IssueDbContext : DbContext
    {
        public IssueDbContext(DbContextOptions<IssueDbContext> options):base(options)
        {

        }

        public DbSet<Issue> Issues { get; set; }
    }
}
