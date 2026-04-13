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
    public class FinancialResponseConfiguration : IEntityTypeConfiguration<FinancialResponse>
    {
        public void Configure(EntityTypeBuilder<FinancialResponse> builder)
        {
            builder.ToTable("FinancialResponses").HasKey(r => r.Id);

            builder.Property(r => r.Decision).IsRequired().HasMaxLength(100);
            builder.Property(r => r.AcceptedAmount).HasColumnType("decimal(18,2)");

            // تطبيق Now Date by default
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");

            // العلاقات
            builder.HasOne(r => r.Request)
                   .WithOne(r => r.Response)
                   .HasForeignKey<FinancialResponse>(r => r.RequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Creator)
                   .WithMany(u => u.FinancialResponses)
                   .HasForeignKey(r => r.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
