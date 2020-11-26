using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mtama.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Mtama.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    
        public DbSet<Mtama.Models.PaymentModel> Payments { get; set; }
        public DbSet<Mtama.Models.MarkerModel> Markers { get; set; }
        public DbSet<Mtama.Models.RegisterViewModel> RegisterViewModel { get; set; }
        public DbSet<Mtama.Models.AggregatorAssociationModel> AggregatorAssociations { get; set; }
    }
}
