using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Examen2Lenguajes.API.Database.Entities
{
    [Table("logs", Schema = "dbo")]
    public class LogEntity : BaseEntity
    {
        // Id
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        // Usuario Id
        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        // Id de la partida
        [Required]
        [Column("journal_entry_id")]
        public Guid? JournalEntryId { get; set; }
        [ForeignKey("IdPartida")]
        public virtual JournalEntryEntity JournalEntry { get; set; }

        // Descripción
        [StringLength(500)]
        [Column("description")]
        public string Description { get; set; }

        // Fecha
        [Column("date")]
        public DateTime Date { get; set; }
    }
}