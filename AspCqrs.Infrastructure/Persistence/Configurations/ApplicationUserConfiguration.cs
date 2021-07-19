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
            
            builder.HasOne(entity => entity.User)
                .WithOne()
                .HasForeignKey<DomainUser>(entity => entity.Id);

            builder.ToTable("AspNetUsers");
        }
    }
}