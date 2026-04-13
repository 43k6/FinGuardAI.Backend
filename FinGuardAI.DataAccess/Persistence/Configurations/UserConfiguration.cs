using FinGuardAI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinGuardAI.DataAccess.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName).IsUnicode().IsRequired().HasMaxLength(50);
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired().HasMaxLength(20);

            // تطبيق Now Date by default
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");

            // العلاقات
            builder.HasOne(u => u.Person)
                   .WithOne()
                   .HasForeignKey<User>(u => u.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
