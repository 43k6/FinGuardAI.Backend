using FinGuardAI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.Persistence.Configurations
{
    public class FinancialRequestConfiguration : IEntityTypeConfiguration<FinancialRequest>
    {
        public void Configure(EntityTypeBuilder<FinancialRequest> builder)
        {
            builder.ToTable("FinancialRequests").HasKey(r => r.Id);

            builder.Property(r => r.RequestName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(r => r.RequestCategory).HasConversion<string>().IsRequired().HasMaxLength(50);

            // تطبيق Now Date by default
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");

            // تطبيق Pending by default
            builder.Property(r => r.State).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");

            // العلاقات
            builder.HasOne(r => r.Creator)
                   .WithMany(u => u.FinancialRequests)
                   .HasForeignKey(r => r.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
