using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tracking.WebApp.Models;

namespace Tracking.WebApp.Data
{
    public class TrackingWebAppContext : DbContext
    {
        public TrackingWebAppContext (DbContextOptions<TrackingWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<Tracking.WebApp.Models.Issue> Issue { get; set; } = default!;
    }
}
