using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ProductServingConfiguration : IEntityTypeConfiguration<ProductServing>
    {
        public void Configure(EntityTypeBuilder<ProductServing> builder)
        {
            builder.ToTable(nameof(ProductServing));
            builder.Property(x => x.Price).IsRequired();
            builder.HasIndex(x => new { x.ProductId, x.ServingId }).IsUnique();
        }
    }
}
