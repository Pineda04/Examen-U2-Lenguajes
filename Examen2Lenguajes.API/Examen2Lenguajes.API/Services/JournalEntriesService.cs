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

namespace Examen2Lenguajes.API.Services
{
    public class JournalEntriesService : IJournalEntriesService
    {
        private readonly ContabilidadContext _context;
        private readonly IAuditService _auditService;
        private readonly ILogger<JournalEntriesService> _logger;
        private readonly IMapper _mapper;

        public JournalEntriesService(ContabilidadContext context,
                                      IAuditService auditService,
                                      ILogger<JournalEntriesService> logger,
                                      IMapper mapper)
        {
            this._context = context;
            this._auditService = auditService;
            this._logger = logger;
            this._mapper = mapper;
        }

        // Obtener todas las partidas contables
        public async Task<ResponseDto<List<JournalEntryDto>>> GetAllAccountsAsync()
        {
            var journalEntries = await _context.JournalEntries
                        .Select(je => new JournalEntryDto
                        {
                            Id = je.Id,
                            Date = je.Date,
                            Description = je.Description,
                            UserId = je.UserId,
                            IsActive = je.IsActive,
                            JournalEntryDetails = je.JournalEntryDetails
                                    .Select(jed => new JournalEntryDetailDto
                                    {
                                        Id = jed.Id,
                                        JournalEntryId = jed.JournalEntryId,
                                        AccountNumber = jed.AccountNumber,
                                        Amount = jed.Amount
                                    }).ToList(),
                            Balances = je.Balances
                                    .Select(b => new BalanceDto
                                    {
                                        Id = b.Id,
                                        Day = b.Day,
                                        Month = b.Month,
                                        Year = b.Year,
                                        Amount = b.Amount,
                                        AccountNumber = b.AccountNumber,
                                        JournalEntryId = b.JournalEntryId
                                    }).ToList()
                        })
                        .ToListAsync();

            // Retornar la respuesta con los datos mapeados
            return new ResponseDto<List<JournalEntryDto>>
            {
                Data = journalEntries,
                Status = true,
                Message = MessagesConstants.RECORDS_FOUND,
                StatusCode = 200
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