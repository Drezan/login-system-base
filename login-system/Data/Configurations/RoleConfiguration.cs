using Microsoft.EntityFrameworkCore;
using login_system.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace login_system.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(50);

            builder.HasIndex(x => x.Name).IsUnique();

            // Seed Data
            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "User"
                },
                new Role
                {
                    Id = 3,
                    Name = "Support"
                }
            );
        }
    }
}