using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntries;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [ApiController]
    [Route("api/entries_details")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class JournalEntriesDetailsController : ControllerBase
    {
        private readonly IJournalEntriesDetailsService _journalEntriesDetailsService;

        public JournalEntriesDetailsController(IJournalEntriesDetailsService journalEntriesDetailsService)
        {
            _journalEntriesDetailsService = journalEntriesDetailsService;
        }   

        // Crear
        [HttpPost]
        [Authorize(Roles = $"{RolesConstant.USER}, {RolesConstant.USER}")]

        public async Task<ActionResult<ResponseDto<JournalEntryDetailDto>>> Create(JournalEntryDetailCreateDto dto)
        {
            var response = await _journalEntriesDetailsService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}