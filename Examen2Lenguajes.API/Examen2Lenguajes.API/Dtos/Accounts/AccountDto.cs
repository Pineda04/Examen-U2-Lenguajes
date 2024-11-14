using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Balances;

namespace Examen2Lenguajes.API.Dtos.Accounts
{
    public class AccountDto
    {
        public string AccountNumber { get; set; }
        
        public string Name { get; set; }

        public string TypeAccount { get; set; }

        public string? ParentAccountId { get; set; }

        public bool AllowMovement { get; set; }

        public ICollection<AccountEntity> ChildAccounts { get; set; } = new List<AccountEntity>();
    }
}