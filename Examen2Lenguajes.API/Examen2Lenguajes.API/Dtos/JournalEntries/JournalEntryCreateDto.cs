using System.ComponentModel.DataAnnotations;

namespace Examen2Lenguajes.API.Dtos.JournalEntries
{
    public class JournalEntryCreateDto
    {        
        // descripción
        [Display(Name = "descripción de la partida")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [StringLength(500, ErrorMessage = "La {0} debe tener menos de {1} caracteres")]
        public string Description { get; set; }

        // Fecha
        [Display(Name = "fecha de la partida")]
        [Required(ErrorMessage = "La {0} es requerida")]
        public DateTime Date { get; set; }

        // usuario id
        [Display(Name = "id del usuario")]
        [Required(ErrorMessage = "El {0} es requerida")]
        public string UserId { get; set; }
    }
}