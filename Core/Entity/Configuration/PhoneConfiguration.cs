using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Entity.Configuration
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable(nameof(Phone));
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.HasIndex(x => new { x.CompanyId, x.PhoneNumber }).IsUnique();
        }
    }
}
