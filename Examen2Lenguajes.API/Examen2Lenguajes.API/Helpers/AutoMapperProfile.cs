
using AutoMapper;
using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.JournalEntries;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;

namespace Examen2Lenguajes.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForAccounts();
            MapsForBalances();
            MapsForJournalEntries();
            MapsForJournalEntriesDetails();
        }

        private void MapsForJournalEntriesDetails()
        {
            CreateMap<JournalEntryDetailEntity, JournalEntryDetailDto>();
            CreateMap<JournalEntryDetailCreateDto, JournalEntryDetailEntity>();
            CreateMap<JournalEntryDetailEditDto, JournalEntryDetailEntity>();
        }

        private void MapsForJournalEntries()
        {
            CreateMap<JournalEntryEntity, JournalEntryDto>();
            CreateMap<JournalEntryCreateDto, JournalEntryEntity>();
            CreateMap<JournalEntryEditDto, JournalEntryEntity>();
        }

        private void MapsForBalances()
        {
            CreateMap<BalanceEntity, BalanceDto>();
        }

        private void MapsForAccounts()
        {
            CreateMap<AccountEntity, AccountDto>();
            CreateMap<AccountCreateDto, AccountEntity>();
            CreateMap<AccountEditDto, AccountEntity>();
        }
    }
}