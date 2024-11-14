using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        // Traer todos
        [HttpGet]
        public async Task<ActionResult<ResponseDto<List<AccountDto>>>> GetAll()
        {
            var response = await _accountsService.GetAllAccountsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Traer por id
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Get(string accountNumber)
        {
            var response = await _accountsService.GetAccountByNumberAsync(accountNumber);
            return StatusCode(response.StatusCode, response);
        }

        // Crear
        [HttpPost]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Create(AccountCreateDto dto)
        {
            var response = await _accountsService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // Editar
        [HttpPut("{accountNumber}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Edit(AccountEditDto dto, string accountNumber)
        {
            var response = await _accountsService.EditAsync(dto, accountNumber);
            return StatusCode(response.StatusCode, response);
        }
        
        // Eliminar
        [HttpDelete("{accountNumber}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Delete(string accountNumber)
        {
            var response = await _accountsService.DeleteAsync(accountNumber);
            return StatusCode(response.StatusCode, response);
        }
    }
}