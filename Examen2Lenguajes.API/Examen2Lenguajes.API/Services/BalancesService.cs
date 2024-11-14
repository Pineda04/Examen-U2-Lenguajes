using AutoMapper;
using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly ContabilidadContext _context;
        private readonly IMapper _mapper;

        public BalancesService(ContabilidadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Obtener todos los balances
        public async Task<ResponseDto<List<BalanceDto>>> GetAllBalancesAsync()
        {
            var balances = await _context.Balances.Include(b => b.Account).ToListAsync();
            var balanceDtos = _mapper.Map<List<BalanceDto>>(balances);

            return new ResponseDto<List<BalanceDto>>
            {
                Data = balanceDtos,
                Status = true,
                Message = MessagesConstants.RECORDS_FOUND,
                StatusCode = 200
            };
        }

        // Obtener un balance por su número de cuenta
        public async Task<ResponseDto<BalanceDto>> GetBalanceByAccountNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = "El número de cuenta no puede estar vacío.",
                    StatusCode = 400
                };
            }

            var balance = await _context.Balances
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

            if (balance == null)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            var balanceDto = _mapper.Map<BalanceDto>(balance);
            return new ResponseDto<BalanceDto>
            {
                Data = balanceDto,
                Status = true,
                Message = MessagesConstants.RECORD_FOUND,
                StatusCode = 200
            };
        }

        // Crear un balance
        public async Task<ResponseDto<BalanceDto>> CreateAsync(BalanceCreateDto dto)
        {
            if (dto.Amount < 0)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = "El balance no puede ser negativo.",
                    StatusCode = 400
                };
            }

            var newBalance = _mapper.Map<BalanceEntity>(dto);
            newBalance.CreatedDate = DateTime.Now;

            // Verificar si la cuenta está bloqueada
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber);

            if (account == null)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = "La cuenta asociada no existe.",
                    StatusCode = 404
                };
            }

            // Solo permitir movimientos si la cuenta no esta bloqueada
            if (!account.AllowMovement)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = "La cuenta está bloqueada para movimientos.",
                    StatusCode = 400
                };
            }

            // Guardar el nuevo balance
            _context.Balances.Add(newBalance);
            await _context.SaveChangesAsync();

            var balanceDto = _mapper.Map<BalanceDto>(newBalance);
            return new ResponseDto<BalanceDto>
            {
                Data = balanceDto,
                Status = true,
                Message = MessagesConstants.CREATE_SUCCESS,
                StatusCode = 201
            };
        }

        // Editar un balance
        public async Task<ResponseDto<BalanceDto>> EditAsync(BalanceEditDto dto, Guid id)
        {
            var balance = await _context.Balances
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (balance == null)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            // Actualizar el balance
            _mapper.Map(dto, balance);
            balance.UpdatedDate = DateTime.Now;

            _context.Balances.Update(balance);
            await _context.SaveChangesAsync();

            var balanceDto = _mapper.Map<BalanceDto>(balance);
            return new ResponseDto<BalanceDto>
            {
                Data = balanceDto,
                Status = true,
                Message = MessagesConstants.UPDATE_SUCCESS,
                StatusCode = 200
            };
        }

        // Eliminar un balance
        public async Task<ResponseDto<BalanceDto>> DeleteAsync(Guid id)
        {
            var balance = await _context.Balances
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (balance == null)
            {
                return new ResponseDto<BalanceDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            _context.Balances.Remove(balance);
            await _context.SaveChangesAsync();

            var balanceDto = _mapper.Map<BalanceDto>(balance);
            return new ResponseDto<BalanceDto>
            {
                Data = balanceDto,
                Status = true,
                Message = MessagesConstants.DELETE_SUCCESS,
                StatusCode = 200
            };
        }
    }
}
