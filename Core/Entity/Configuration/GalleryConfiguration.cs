using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class GalleryConfiguration : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.ToTable(nameof(Gallery));
            builder.Property(x => x.Description).HasMaxLength(512);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x => x.CreatedOn).IsRequired();
        }
    }
}
