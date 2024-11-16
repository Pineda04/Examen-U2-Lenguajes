using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Common;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<ResponseDto<PaginationDto<List<AccountDto>>>> GetAllAccountsAsync(
            string searchTerm = "", int page = 1
        );
        Task<ResponseDto<AccountDto>> GetAccountByNumberAsync(string accountNumber);
        Task<ResponseDto<AccountDto>> CreateAsync(AccountCreateDto dto);
        Task<ResponseDto<AccountDto>> EditAsync(AccountEditDto dto, string accountNumber);
        Task<ResponseDto<AccountDto>> DeleteAsync(string accountNumber);
    }
}