using AutoMapper;
using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly ContabilidadContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;

        public AccountsService(ContabilidadContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }

        // Obtener todas las cuentas
        public async Task<ResponseDto<PaginationDto<List<AccountDto>>>> GetAllAccountsAsync(
            string searchTerm = "",
            int page = 1)
        {
            int startIndex = (page - 1) * PAGE_SIZE;

            var accountsQuery = _context.Accounts
                .Include(a => a.ChildAccounts)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accountsQuery = accountsQuery
                    .Where(a => (a.Name + " " + a.AccountNumber)
                    .ToLower().Contains(searchTerm.ToLower()));
            }

            int totalAccounts = await accountsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalAccounts / PAGE_SIZE);

            var accountsEntity = await accountsQuery
                .OrderBy(a => a.Name)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();

            var accountsDto = _mapper.Map<List<AccountDto>>(accountsEntity);

            return new ResponseDto<PaginationDto<List<AccountDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = MessagesConstants.RECORDS_FOUND,
                Data = new PaginationDto<List<AccountDto>>
                {
                    CurrentPage = page,
                    PageSize = PAGE_SIZE,
                    TotalItems = totalAccounts,
                    TotalPages = totalPages,
                    Items = accountsDto,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages
                }
            };
        }

        // Obtener una cuenta por su número
        public async Task<ResponseDto<AccountDto>> GetAccountByNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = "El número de cuenta no puede estar vacío.",
                    StatusCode = 400
                };
            }

            var account = await _context.Accounts.Include(a => a.ChildAccounts)
                        .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            var accountDto = _mapper.Map<AccountDto>(account);
            return new ResponseDto<AccountDto>
            {
                Data = accountDto,
                Status = true,
                Message = MessagesConstants.RECORD_FOUND,
                StatusCode = 200
            };
        }


        public async Task<ResponseDto<AccountDto>> CreateAsync(AccountCreateDto dto)
        {
            // Verificar si la cuenta ya existe
            if (await _context.Accounts.AnyAsync(a => a.AccountNumber == dto.AccountNumber))
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = "La cuenta con este número ya existe.",
                    StatusCode = 400
                };
            }

            var newAccount = _mapper.Map<AccountEntity>(dto);
            newAccount.CreatedDate = DateTime.Now;

            // Determinar el padre de la cuenta
            var potentialParent = await FindParentAccountAsync(newAccount.AccountNumber);

            if (potentialParent != null)
            {
                newAccount.ParentAccountId = potentialParent.AccountNumber;
                newAccount.ParentAccount = potentialParent;

                // Asignar el tipo de cuenta de acuerdo al padre
                newAccount.TypeAccount = potentialParent.TypeAccount;

                // Después de identificar el padre, verificamos si tiene cuentas hijas, y actualizamos la propiedad AllowMovement
                if (potentialParent.ChildAccounts.Any())
                {
                    newAccount.AllowMovement = false; // No permitir movimientos
                }
                else
                {
                    newAccount.AllowMovement = true; // Permitir movimientos
                }

                // Actualizamos la cuenta padre para reflejar que ahora tiene una cuenta hija
                potentialParent.AllowMovement = false; // No permitir movimientos en la cuenta padre si tiene hijos
                _context.Accounts.Update(potentialParent); // actualizamos la cuenta padre
            }
            else
            {
                // Si la cuenta es super padre su tipo sera su nombre
                newAccount.TypeAccount = newAccount.Name; // El tipo de cuenta es igual al nombre para cuentas super padres
                newAccount.AllowMovement = true; // Si no tiene padre permitir movimientos
            }

            // Guardamos la nueva cuenta
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            // Verificamos si la cuenta recién creada tiene hijos
            var accountWithChildren = await _context.Accounts.Include(a => a.ChildAccounts)
                                    .FirstOrDefaultAsync(a => a.AccountNumber == newAccount.AccountNumber);

            if (accountWithChildren?.ChildAccounts.Any() == true)
            {
                accountWithChildren.AllowMovement = false; // Si tiene cuentas hijas, no permitir movimiento
                _context.Accounts.Update(accountWithChildren);
                await _context.SaveChangesAsync();
            }

            var accountDto = _mapper.Map<AccountDto>(newAccount);
            return new ResponseDto<AccountDto>
            {
                Data = accountDto,
                Status = true,
                Message = MessagesConstants.CREATE_SUCCESS,
                StatusCode = 201
            };
        }

        // Editar una cuenta
        public async Task<ResponseDto<AccountDto>> EditAsync(AccountEditDto dto, string accountNumber)
        {
            var account = await _context.Accounts.Include(a => a.ChildAccounts)
                            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            // Actualizar los datos de la cuenta
            _mapper.Map(dto, account);
            account.UpdatedDate = DateTime.Now;

            // Recalcular la cuenta padre
            var potentialParent = await FindParentAccountAsync(account.AccountNumber);
            account.ParentAccountId = potentialParent?.AccountNumber;

            // Verificar si la cuenta tiene hijas en tal caso no permitir movimientos
            var isParent = await _context.Accounts.AnyAsync(a => a.ParentAccountId == account.AccountNumber);
            account.AllowMovement = !isParent; // Si es padre no permite movimiento

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();

            // Verificar si la cuenta es un padre ahora que se han guardado las cuentas hijas
            if (account.ChildAccounts.Any())
            {
                account.AllowMovement = false; // Si tiene cuentas hijas, no permitir movimiento
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }

            var accountDto = _mapper.Map<AccountDto>(account);
            return new ResponseDto<AccountDto>
            {
                Data = accountDto,
                Status = true,
                Message = MessagesConstants.UPDATE_SUCCESS,
                StatusCode = 200
            };
        }


        // Eliminar una cuenta
        public async Task<ResponseDto<AccountDto>> DeleteAsync(string accountNumber)
        {
            var account = await _context.Accounts.Include(a => a.ChildAccounts)
                        .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = MessagesConstants.RECORD_NOT_FOUND,
                    StatusCode = 404
                };
            }

            // Verificar si la cuenta tiene cuentas hijas
            if (account.ChildAccounts.Any())
            {
                return new ResponseDto<AccountDto>
                {
                    Status = false,
                    Message = "No se puede eliminar una cuenta que tiene cuentas hijas.",
                    StatusCode = 400
                };
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            var accountDto = _mapper.Map<AccountDto>(account);
            return new ResponseDto<AccountDto>
            {
                Data = accountDto,
                Status = true,
                Message = MessagesConstants.DELETE_SUCCESS,
                StatusCode = 200
            };
        }

        // Metodo para encontrar el padre de una cuenta
        private async Task<AccountEntity?> FindParentAccountAsync(string accountNumber)
        {
            var potentialParents = await _context.Accounts
                .Where(a => a.AccountNumber.Length < accountNumber.Length)
                .OrderByDescending(a => a.AccountNumber.Length)
                .ToListAsync();

            foreach (var parent in potentialParents)
            {
                if (accountNumber.StartsWith(parent.AccountNumber))
                {
                    return parent;
                }
            }
            return null;
        }
    }
}