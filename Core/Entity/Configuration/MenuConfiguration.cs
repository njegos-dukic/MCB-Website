using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable(nameof(Menu));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(127);
            builder.Property(x => x.Description).HasMaxLength(1023);
            builder.HasIndex(x => new { x.Name, x.CompanyId }).IsUnique();
        }
    }
}
