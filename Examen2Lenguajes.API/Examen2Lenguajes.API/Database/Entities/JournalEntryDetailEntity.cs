using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Database.Entities
{
    [Table("journal_entries_details", Schema = "dbo")]
    public class JournalEntryDetailEntity : BaseEntity
    {
        // Id
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        // Id de la partida 
        [Required]
        [Column("journal_entry_id")]
        public Guid JournalEntryId { get; set; }
        [ForeignKey(nameof(JournalEntryId))]
        public virtual JournalEntryEntity JournalEntry { get; set; }

        // Numero de la cuenta
        [Required]
        [Column("account_number")]
        public string AccountNumber { get; set; }
        [ForeignKey(nameof(AccountNumber))]
        public AccountEntity Account { get; set; }

        // Monto
        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        public virtual IdentityUser CreatedByUser { get; set; }
        public virtual IdentityUser UpdatedByUser { get; set; }
    }
}