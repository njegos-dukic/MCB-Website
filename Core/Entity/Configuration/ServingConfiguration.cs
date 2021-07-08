using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class ServingConfiguration : IEntityTypeConfiguration<Serving>
    {
        public void Configure(EntityTypeBuilder<Serving> builder)
        {
            builder.ToTable(nameof(Serving));
            builder.Property(x => x.Name).HasMaxLength(255).IsUnicode().IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
