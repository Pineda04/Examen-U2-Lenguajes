using Examen2Lenguajes.API.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Examen2Lenguajes.API.Database.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .HasPrincipalKey(e => e.Id);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .HasPrincipalKey(e => e.Id);
        }
    }
}