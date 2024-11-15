using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Examen2Lenguajes.API.Database.Configuration
{
    public class BalanceConfiguration : IEntityTypeConfiguration<BalanceEntity>
    {
        public void Configure(EntityTypeBuilder<BalanceEntity> builder)
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