using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Core.Entity.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));

            builder.Property(x => x.City).HasMaxLength(63)
                                         .HasDefaultValue(Constants.DefaultCity)
                                         .IsRequired();
            builder.Property(x => x.CountryName).HasMaxLength(63)
                                                .HasDefaultValue(Constants.BosniaAndHercegovina)
                                                .IsRequired();
            builder.Property(x => x.Street).HasMaxLength(127).IsRequired();
            builder.Property(x => x.PostalCode).HasMaxLength(5)
                                               .IsFixedLength()
                                               .HasDefaultValue(Constants.PostalCodeBanjaLuka)
                                               .IsRequired();
            builder.Property(x => x.IsDelivered).HasDefaultValue(false).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.Property(x => x.OrderedOn).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.TotalCost).IsRequired();
        }
    }
}
