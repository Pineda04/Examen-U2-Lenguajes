using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Examen2Lenguajes.API.Database.Entities
{
    [Table("balances", Schema = "dbo")]
    public class BalanceEntity : BaseEntity
    {
        // Id
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        // dia
        [Required]
        [Column("day")]
        public int Day { get; set; }

        // Mes
        [Required]
        [Column("month")]
        public int Month { get; set; }

        // Año
        [Required]
        [Column("year")]
        public int Year { get; set; }

        // Monto
        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        // Numero de cuenta
        [Required]
        [Column("account_number")]
        public string AccountNumber { get; set; }
        [ForeignKey(nameof(AccountNumber))]
        public AccountEntity Account { get; set; }

        // Id de la partida
        [Required]
        [Column("journal_entry_id")]
        public Guid JournalEntryId { get; set; }
        [ForeignKey(nameof(JournalEntryId))]
        public JournalEntryEntity JournalEntry { get; set; }

        public virtual IdentityUser CreatedByUser { get; set; }
        public virtual IdentityUser UpdatedByUser { get; set; }
    }
}