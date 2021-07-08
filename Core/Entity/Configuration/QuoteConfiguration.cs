using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Configuration
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.ToTable(nameof(Quote));
            builder.Property(x => x.QuoteText).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Author).HasMaxLength(100);
            builder.Property(x => x.CreatedOn).IsRequired();
        }
    }
}
