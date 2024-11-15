using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IJournalEntriesDetailsService
    {
        Task<ResponseDto<JournalEntryDetailDto>> CreateAsync(JournalEntryDetailCreateDto dto);
    }
}