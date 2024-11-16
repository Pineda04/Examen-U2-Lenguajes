using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Examen2Lenguajes.API.Constants;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos.Balances;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntries;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Examen2Lenguajes.API.Services
{
    public class JournalEntriesService : IJournalEntriesService
    {
        private readonly ContabilidadContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<JournalEntriesService> _logger;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;

        public JournalEntriesService(ContabilidadContext context,
                                      IAuditService auditService,
                                      ILogger<JournalEntriesService> logger,
                                      IMapper mapper,
                                      IConfiguration configuration)
        {
            this._context = context;
            this._auditService = auditService;
            this._logger = logger;
            this._mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }

        // Obtener todas las partidas contables
        public async Task<ResponseDto<PaginationDto<List<JournalEntryDto>>>> GetAllJournalsAsync(
            string searchTerm = "", int page = 1
            )
        {
            int startIndex = (page - 1) * PAGE_SIZE;

            var journalEntriesQuery = _context.JournalEntries
                .Include(je => je.JournalEntryDetails)
                .Include(je => je.Balances)
                .AsQueryable();

            int totalJournalEntries = await journalEntriesQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalJournalEntries / PAGE_SIZE);

            var journalEntriesEntity = await journalEntriesQuery
                .OrderBy(je => je.Date)
                .Skip(startIndex)
                .Take(PAGE_SIZE)
                .ToListAsync();

            var journalEntriesDto = _mapper.Map<List<JournalEntryDto>>(journalEntriesEntity);

            return new ResponseDto<PaginationDto<List<JournalEntryDto>>>
            {
                StatusCode = 200,
                Status = true,
                Message = MessagesConstants.RECORDS_FOUND,
                Data = new PaginationDto<List<JournalEntryDto>>
                {
                    CurrentPage = page,
                    PageSize = PAGE_SIZE,
                    TotalItems = totalJournalEntries,
                    TotalPages = totalPages,
                    Items = journalEntriesDto,
                    HasPreviousPage = page > 1,
                    HasNextPage = page < totalPages
                }
            };
        }


        public async Task<ResponseDto<JournalEntryDto>> CreateAsync(JournalEntryCreateDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var journalEntryEntity = _mapper.Map<JournalEntryEntity>(dto);

                    journalEntryEntity.UserId = _auditService.GetUserId();

                    _context.JournalEntries.Add(journalEntryEntity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var journalEntryDto = _mapper.Map<JournalEntryDto>(journalEntryEntity);

                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 201,
                        Status = true,
                        Message = MessagesConstants.CREATE_SUCCESS,
                        Data = journalEntryDto
                    };
                }
                catch (Exception e)
                {
                    // Rollback en caso de error
                    await transaction.RollbackAsync();
                    _logger.LogError(e, MessagesConstants.CREATE_ERROR);

                    return new ResponseDto<JournalEntryDto>
                    {
                        StatusCode = 500,
                        Status = false,
                        Message = MessagesConstants.CREATE_ERROR
                    };
                }
            }
        }

    }
}