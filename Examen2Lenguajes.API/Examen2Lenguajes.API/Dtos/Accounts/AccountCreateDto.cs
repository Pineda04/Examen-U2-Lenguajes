using System.ComponentModel.DataAnnotations;

namespace Examen2Lenguajes.API.Dtos.Accounts
{
    public class AccountCreateDto
    {
        // Numero de cuenta
        [Display(Name = "numero de cuenta")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string AccountNumber { get; set; }
        
        // Nombre
        [Display(Name = "nombre de la cuenta")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [StringLength(75, ErrorMessage = "El {0} debe tener menos de {1} caracteres.")]
        public string Name { get; set; }

        // tipo de cuenta
        [Display(Name = "tipo de la cuenta")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [StringLength(75, ErrorMessage = "El {0} debe tener menos de {1} caracteres.")]
        public string TypeAccount { get; set; }

        // permite movimiento
        public bool AllowMovement { get; set; }
    }
}