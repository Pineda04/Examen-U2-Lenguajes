using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Examen2Lenguajes.API.Database.Entities
{
    [Table("accounts", Schema = "dbo")]
    public class AccountEntity : BaseEntity
    {
        // Numero de la cuenta
        [Key]
        [Column("account_number")]
        public string AccountNumber { get; set; }

        // Nombre de la cuenta
        [Required]
        [Column("name")]
        [StringLength(75)]
        public string Name { get; set; }

        // Tipo de la cuenta
        [Required]
        [Column("type_account")]
        [StringLength(75)]
        public string TypeAccount { get; set; }

        // Id de la cuenta padre
        [Column("parent_account_id")]
        public string? ParentAccountId { get; set; }
        [ForeignKey(nameof(ParentAccountId))]
        [JsonIgnore]
        public virtual AccountEntity ParentAccount { get; set; }

        // Permitir movimiento
        [Column("allow_movement")]
        public bool AllowMovement { get; set; }

        [JsonIgnore]
        public ICollection<AccountEntity> ChildAccounts { get; set; } = new List<AccountEntity>();

        public virtual IdentityUser CreatedByUser { get; set; }
        public virtual IdentityUser UpdatedByUser { get; set; }
    }
}