using System.ComponentModel.DataAnnotations;

namespace Examen2Lenguajes.API.Dtos.JournalEntriesDetails
{
    public class JournalEntryDetailCreateDto
    {   
        // id de la partida
        [Display(Name = "id de la partida")]
        [Required(ErrorMessage = "El {0} es requerida.")]
        public Guid JournalEntryId { get; set; }

        // numero de cuenta
        [Display(Name = "numero de cuenta")]
        [Required(ErrorMessage = "El {0} es requerida.")]
        public string AccountNumber { get; set; }

        // Monto
        [Display(Name = "monto de la partida")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public decimal Amount { get; set; }
    }
}