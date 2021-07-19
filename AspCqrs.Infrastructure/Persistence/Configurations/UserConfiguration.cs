using AspCqrs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspCqrs.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<DomainUser>
    {
        public void Configure(EntityTypeBuilder<DomainUser> builder)
        {
            builder.Property(entity => entity.UserName)
                .HasColumnType("nvarchar(max)")
                .HasColumnName(nameof(DomainUser.UserName));
            
            builder.Property(entity => entity.Created)
                .HasColumnName(nameof(DomainUser.Created));

            builder.ToTable("AspNetUsers");
        }
    }
}