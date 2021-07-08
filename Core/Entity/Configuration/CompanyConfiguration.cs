using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));
            builder.Property(x => x.ApiKey).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.Fax).HasMaxLength(15);

            builder.Property(x => x.PostalCode).HasMaxLength(5).HasDefaultValue(Constants.PostalCodeBanjaLuka);
            builder.Property(x => x.Address).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Coordinates).HasMaxLength(255).IsRequired();

            builder.Property(x => x.ShopDescription).HasMaxLength(1023).IsRequired(false);

            builder.HasIndex(x => new { x.Name }).IsUnique();
        }
    }
}
