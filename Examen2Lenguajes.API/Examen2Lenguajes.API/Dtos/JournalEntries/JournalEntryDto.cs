using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;

namespace Examen2Lenguajes.API.Dtos.JournalEntries
{
    public class JournalEntryDto
    {
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public bool IsActive { get; set; }

        public ICollection<JournalEntryDetailDto> JournalEntryDetails { get; set; } = new List<JournalEntryDetailDto>();

        public ICollection<BalanceDto> Balances { get; set; } = new List<BalanceDto>();
    }
}