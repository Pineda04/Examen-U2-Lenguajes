using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [ApiController]
    [Route("api/balances")]
    public class BalancesController : ControllerBase
    {
        private readonly IBalancesService _balancesService;
        public BalancesController(IBalancesService balancesService)
        {
            _balancesService = balancesService;
        }

        // Traer todos
        [HttpGet]
        public async Task<ActionResult<ResponseDto<List<BalanceDto>>>> GetAll()
        {
            var response = await _balancesService.GetAllBalancesAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Traer por id
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<ResponseDto<BalanceDto>>> Get(string accountNumber)
        {
            var response = await _balancesService.GetBalanceByAccountNumberAsync(accountNumber);
            return StatusCode(response.StatusCode, response);
        }

        // Crear
        [HttpPost]
        public async Task<ActionResult<ResponseDto<BalanceDto>>> Create(BalanceCreateDto dto)
        {
            var response = await _balancesService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // Editar
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<BalanceDto>>> Edit(BalanceEditDto dto, Guid id)
        {
            var response = await _balancesService.EditAsync(dto, id);
            return StatusCode(response.StatusCode, response);
        }
        
        // Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<BalanceDto>>> Delete(Guid id)
        {
            var response = await _balancesService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}