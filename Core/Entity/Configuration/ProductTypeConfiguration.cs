using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.ToTable(nameof(ProductType));
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
