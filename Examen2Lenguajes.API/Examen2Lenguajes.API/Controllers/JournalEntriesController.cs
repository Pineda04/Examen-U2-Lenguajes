using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntries;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [ApiController]
    [Route("api/journal_entries")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IJournalEntriesService _journalEntriesService;

        public JournalEntriesController(IJournalEntriesService journalEntriesService)
        {
            _journalEntriesService = journalEntriesService;
        }   

        // Traer todos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<List<JournalEntryDto>>>> GetAll()
        {
            var response = await _journalEntriesService.GetAllAccountsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Crear
        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.USER}, {RolesConstant.USER}")]

        public async Task<ActionResult<ResponseDto<JournalEntryDto>>> Create(JournalEntryCreateDto dto)
        {
            var response = await _journalEntriesService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}