using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saraha.Api.Data.Models.Entities;

namespace Saraha.Api.Data.EntityConfigurations
{
    public class UserMessagesConfiguration : IEntityTypeConfiguration<UserMessages>
    {
        public void Configure(EntityTypeBuilder<UserMessages> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.Messages)
                .HasForeignKey(e => e.UserId);
            builder.Property(e => e.Message).IsRequired();
        }
    }
}
