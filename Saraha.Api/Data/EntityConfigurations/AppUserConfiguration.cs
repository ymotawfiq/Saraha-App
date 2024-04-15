using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(e => e.FirstName).IsRequired().HasColumnName("First Name");
            builder.Property(e => e.LastName).IsRequired().HasColumnName("Last Name");
        }
    }
}
