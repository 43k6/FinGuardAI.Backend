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
            builder.Property(u => u.PersonId).IsRequired();

            // تطبيق Now Date by default
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Person)
           .WithOne(x => x.User)
           .HasForeignKey<User>(x => x.PersonId)
           .IsRequired(false);
                  
        }
    }
}
