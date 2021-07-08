using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.ToTable(nameof(MediaFile));
            builder.Property(x => x.Uri).HasMaxLength(512).IsRequired();
            builder.Property(x => x.IsThumbnail).HasDefaultValue(false);

        }
    }
}
