using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Database
{
    public class ContabilidadContext : DbContext
    {
        private readonly IAuthService _authService;

        public ContabilidadContext(DbContextOptions<ContabilidadContext> options, IAuthService authService) : base(options)
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

            // Configuración de AccountEntity
            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.HasKey(e => e.AccountNumber);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(75);
                entity.Property(e => e.TypeAccount).IsRequired().HasMaxLength(75);
                entity.HasMany(e => e.ChildAccounts)
                      .WithOne(e => e.ParentAccount)
                      .HasForeignKey(e => e.ParentAccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de BalanceEntity
            modelBuilder.Entity<BalanceEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Day).IsRequired();
                entity.Property(e => e.Month).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.HasOne(e => e.Account)
                      .WithMany()
                      .HasForeignKey(e => e.AccountNumber)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de JournalEntryEntity
            modelBuilder.Entity<JournalEntryEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsActive).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de JournalEntryDetailEntity
            modelBuilder.Entity<JournalEntryDetailEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired();
                entity.HasOne(e => e.JournalEntry)
                      .WithMany(e => e.JournalEntryDetails)
                      .HasForeignKey(e => e.JournalEntryId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Account)
                      .WithMany()
                      .HasForeignKey(e => e.AccountNumber)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<JournalEntryEntity> JournalEntries { get; set; }
        public DbSet<JournalEntryDetailEntity> JournalEntryDetails { get; set; }
        public DbSet<BalanceEntity> Balances { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}