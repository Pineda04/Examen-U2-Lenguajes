using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.Common;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IBalancesService
    {
        Task<ResponseDto<List<BalanceDto>>> GetAllBalancesAsync();
        Task<ResponseDto<BalanceDto>> GetBalanceByAccountNumberAsync(string accountNumber);
        Task<ResponseDto<BalanceDto>> CreateAsync(BalanceCreateDto dto);
        Task<ResponseDto<BalanceDto>> EditAsync(BalanceEditDto dto, Guid id);
        Task<ResponseDto<BalanceDto>> DeleteAsync(Guid id);
    }
}