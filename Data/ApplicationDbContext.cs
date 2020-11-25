using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mtama.Models;
using Mtama.Models.HomeViewModels;
using Mtama.Models.AccountViewModels;
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
    
        public DbSet<Mtama.Models.HomeViewModels.PaymentModel> Payments { get; set; }
        public DbSet<Mtama.Models.HomeViewModels.MarkerModel> Markers { get; set; }
        public DbSet<Mtama.Models.AccountViewModels.RegisterViewModel> RegisterViewModel { get; set; }
        public DbSet<Mtama.Models.HomeViewModels.AggregatorAssociationModel> AggregatorAssociations { get; set; }
    }
}
