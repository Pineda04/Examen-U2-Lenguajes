using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Database
{
    public class LogsContext : DbContext
    {
        private readonly IAuthService _authService;

        public LogsContext(DbContextOptions<LogsContext> options, IAuthService authService) : base(options)
        {
            _authService = authService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added || e.State == EntityState.Modified
            ));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;

                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = _authService.GetUserId();
                        entity.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        entity.UpdatedBy = _authService.GetUserId();
                        entity.UpdatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de LogEntity
            modelBuilder.Entity<LogEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.JournalEntry)
                      .WithMany()
                      .HasForeignKey(e => e.JournalEntryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<LogEntity> Logs;
    }
}