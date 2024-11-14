using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Common;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<ResponseDto<List<AccountDto>>> GetAllAccountsAsync();
        Task<ResponseDto<AccountDto>> GetAccountByNumberAsync(string accountNumber);
        Task<ResponseDto<AccountDto>> CreateAsync(AccountCreateDto dto);
        Task<ResponseDto<AccountDto>> EditAsync(AccountEditDto dto, string accountNumber);
        Task<ResponseDto<AccountDto>> DeleteAsync(string accountNumber);
    }
}