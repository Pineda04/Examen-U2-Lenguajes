using Examen2Lenguajes.API.Database.Entities;

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
    }
}