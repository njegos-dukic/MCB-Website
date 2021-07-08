using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable(nameof(Ingredient));
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.HasIndex(x => new { x.Name, x.ProductId }).IsUnique();
        }
    }
}
