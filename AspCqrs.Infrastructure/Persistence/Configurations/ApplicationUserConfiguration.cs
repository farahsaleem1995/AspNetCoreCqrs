using AspCqrs.Domain.Entities;
using AspCqrs.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspCqrs.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(entity => entity.UserName)
                .HasColumnType("nvarchar(max)")
                .HasColumnName(nameof(ApplicationUser.UserName));
            
            builder.Property(entity => entity.Created)
                .HasColumnName(nameof(ApplicationUser.Created));
            
            builder.Property(entity => entity.LastModified)
                .HasColumnName(nameof(ApplicationUser.LastModified));

            builder.HasOne(entity => entity.User)
                .WithOne()
                .HasForeignKey<DomainUser>(entity => entity.Id);

            builder.ToTable("AspNetUsers");
        }
    }
}