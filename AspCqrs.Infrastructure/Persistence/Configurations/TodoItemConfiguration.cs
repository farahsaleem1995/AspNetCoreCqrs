using AspCqrs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspCqrs.Infrastructure.Persistence.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(entity => entity.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(entity => entity.Priority)
                .HasConversion<string>();

            builder.Ignore(entity => entity.DomainEvents);
        }
    }
}