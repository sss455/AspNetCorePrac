using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreEntity.Models;

public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Comments")
          .HasKey(b => b.Code);
        builder.Property(e => e.Body)
          .HasColumnName("Message")
          .HasMaxLength(150);
    }
}