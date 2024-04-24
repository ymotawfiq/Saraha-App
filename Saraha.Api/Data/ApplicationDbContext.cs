using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saraha.Api.Data.EntityConfigurations;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			// seed on first migration
            //SeedRoles(builder);
            ApplyConfigurations(builder);
        }

        // seed on first migration
        //private void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityRole>().HasData
        //        (
        //            new IdentityRole() { ConcurrencyStamp = "1", Name = "Admin", NormalizedName = "Admin"},
        //            new IdentityRole() { ConcurrencyStamp = "2", Name = "User", NormalizedName = "User" }
        //        );
        //}

        private void ApplyConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserConfiguration())
                   .ApplyConfiguration(new UserMessagesConfiguration());
        }


        public DbSet<UserMessages> UserMessages { get; set; }

    }
}
