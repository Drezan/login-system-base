using Microsoft.EntityFrameworkCore;
using login_system.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace login_system.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Password).IsRequired().HasMaxLength(500);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.CreatedAt).IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}