using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntries;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IJournalEntriesService
    {
        Task<ResponseDto<PaginationDto<List<JournalEntryDto>>>> GetAllJournalsAsync(
            string searchTerm = "", int page = 1
         );

        Task<ResponseDto<JournalEntryDto>> CreateAsync(JournalEntryCreateDto dto);
    }
}