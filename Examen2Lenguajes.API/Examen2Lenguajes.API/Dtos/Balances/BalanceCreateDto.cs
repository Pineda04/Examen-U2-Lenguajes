using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Lenguajes.API.Dtos.Balances
{
    public class BalanceCreateDto
    {
        // Dia
        [Display(Name = "dia")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public int Day { get; set; }

        // mes
        [Display(Name = "mes")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public int Month { get; set; }

        // año
        [Display(Name = "año")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public int Year { get; set; }

        // Monto
        [Display(Name = "monto")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public decimal Amount { get; set; }

        // numero de cuenta
        [Display(Name = "numero de cuenta")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string AccountNumber { get; set; }

        // id de la partida
        [Display(Name = "id de la partida")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public Guid JournalEntryId { get; set; }
    }
}