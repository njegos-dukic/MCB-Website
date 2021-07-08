using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class WholesaleConfiguration : IEntityTypeConfiguration<Wholesale>
    {
        public void Configure(EntityTypeBuilder<Wholesale> builder)
        {
            builder.ToTable(nameof(Wholesale));
            builder.Property(x => x.Address).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Coordinates).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(127).IsRequired();
        }
    }
}
