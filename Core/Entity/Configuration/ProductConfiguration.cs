using Core.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));

            builder.Property(x => x.Name).HasMaxLength(127).IsRequired();
            builder.Property(x => x.IsInStock).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.PhotoUri).HasMaxLength(512).HasDefaultValue(PathBuilder.DefaultProductImage);
            builder.Property(x => x.Description).HasMaxLength(1023);
            builder.Property(x => x.HexColor).HasMaxLength(7).HasDefaultValue(Constants.White);
            builder.HasIndex(x => new { x.Name }).IsUnique();
        }
    }
}
