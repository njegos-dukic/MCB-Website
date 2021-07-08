using Core.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ShopProductServingConfiguration : IEntityTypeConfiguration<ShopProductServing>
    {
        public void Configure(EntityTypeBuilder<ShopProductServing> builder)
        {
            builder.ToTable(nameof(ShopProductServing));
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.PhotoUri).HasMaxLength(512).HasDefaultValue(PathBuilder.DefaultProductImage);
            builder.HasIndex(x => new { x.ProductId, x.ServingId }).IsUnique();
        }
    }
}
