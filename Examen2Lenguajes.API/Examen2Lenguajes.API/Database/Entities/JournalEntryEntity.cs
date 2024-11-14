using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examen2Lenguajes.API.Database.Entities
{
    [Table("journal_entries", Schema = "dbo")]
    public class JournalEntryEntity : BaseEntity
    {
        // Id
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        // Fecha 
        [Column("date")]
        [Required]
        public DateTime Date { get; set; }

        // Descripción
        [Required]
        [StringLength(500)]
        [Column("description")]
        public string Description { get; set; }

        // Id del usuario
        [Column("user_id")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        // Activa
        [Column("is_active")]
        public bool IsActive { get; set; }

        // Relación con detalles de la partida
        public ICollection<JournalEntryDetailEntity> JournalEntryDetails { get; set; } = new List<JournalEntryDetailEntity>();
        public ICollection<BalanceEntity> Balances { get; set; } = new List<BalanceEntity>();
    }
}