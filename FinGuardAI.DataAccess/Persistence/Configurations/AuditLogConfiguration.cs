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
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs").HasKey(a => a.Id);

            builder.Property(a => a.ActionType).HasConversion<string>().IsRequired().HasMaxLength(100);

            // تطبيق Now Date by default
            builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");

            // العلاقات
            builder.HasOne(a => a.Creator)
                   .WithMany(u => u.AuditLogs)
                   .HasForeignKey(a => a.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
