namespace Examen2Lenguajes.API.Dtos.Balances
{
    public class BalanceDto
    {
        public Guid Id { get; set; }

        public int Day { get; set; }
        
        public int Month { get; set; }

        public int Year { get; set; }

        public decimal Amount { get; set; }

        public string AccountNumber { get; set; }
        
        public Guid JournalEntryId { get; set; }
    }
}