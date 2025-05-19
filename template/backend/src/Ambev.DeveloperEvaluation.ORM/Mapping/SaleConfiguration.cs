using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Date).IsRequired();

            builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.BranchName).IsRequired().HasMaxLength(100);

            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.IsCancelled).IsRequired();

            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey(x => x.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
