using Core.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable(nameof(Event));
            builder.Property(x => x.Description).HasMaxLength(2047);
            builder.Property(x => x.Price).HasDefaultValue(Constants.Free);
            builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PhotoUri).HasMaxLength(255).HasDefaultValue(PathBuilder.DefaultEventImage).IsRequired();
            builder.Property(x => x.Location).IsRequired();
        }
    }
}
