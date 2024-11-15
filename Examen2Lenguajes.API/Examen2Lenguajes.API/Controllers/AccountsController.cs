using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Dtos.Accounts;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        // Traer todos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<List<AccountDto>>>> GetAll()
        {
            var response = await _accountsService.GetAllAccountsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Traer por id
        [HttpGet("{accountNumber}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Get(string accountNumber)
        {
            var response = await _accountsService.GetAccountByNumberAsync(accountNumber);
            return StatusCode(response.StatusCode, response);
        }

        // Crear
        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.USER}, {RolesConstant.USER}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Create(AccountCreateDto dto)
        {
            var response = await _accountsService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // Editar
        [HttpPut("{accountNumber}")]
        [Authorize(Roles = $"{RolesConstant.USER}, {RolesConstant.USER}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Edit(AccountEditDto dto, string accountNumber)
        {
            var response = await _accountsService.EditAsync(dto, accountNumber);
            return StatusCode(response.StatusCode, response);
        }
        
        // Eliminar
        [HttpDelete("{accountNumber}")]
        [Authorize(Roles = $"{RolesConstant.USER}, {RolesConstant.USER}")]
        public async Task<ActionResult<ResponseDto<AccountDto>>> Delete(string accountNumber)
        {
            var response = await _accountsService.DeleteAsync(accountNumber);
            return StatusCode(response.StatusCode, response);
        }
    }
}