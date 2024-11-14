namespace Examen2Lenguajes.API.Dtos.JournalEntriesDetails
{
    public class JournalEntryDetailDto
    {
        public Guid Id { get; set; }
        
        public Guid JournalEntryId { get; set; }

        public string AccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}