using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ShopAmountConfiguration : IEntityTypeConfiguration<ShopAmount>
    {
        public void Configure(EntityTypeBuilder<ShopAmount> builder)
        {
            builder.ToTable(nameof(ShopAmount));
            builder.Property(x => x.PackageAmount).IsRequired();
            builder.Property(x => x.IncrementAmount).IsRequired();
        }
    }
}
