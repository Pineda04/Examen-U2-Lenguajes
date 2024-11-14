using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Balances;

namespace Examen2Lenguajes.API.Dtos.JournalEntries
{
    public class JournalEntryDto
    {
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public bool IsActive { get; set; }

        public ICollection<JournalEntryDetailEntity> JournalEntryDetails { get; set; } = new List<JournalEntryDetailEntity>();

        public ICollection<BalanceDto> Balances { get; set; }
    }
}