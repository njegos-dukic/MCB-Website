using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class SocialMediaConfiguration : IEntityTypeConfiguration<SocialMedia>
    {
        public void Configure(EntityTypeBuilder<SocialMedia> builder)
        {
            builder.ToTable(nameof(SocialMedia));
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}
