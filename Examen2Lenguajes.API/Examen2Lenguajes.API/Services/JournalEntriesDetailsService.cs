using System;
using System.Threading.Tasks;
using AutoMapper;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Database.Entities;
using Examen2Lenguajes.API.Dtos;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Dtos.JournalEntriesDetails;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API.Services
{
    public class JournalEntriesDetailsService : IJournalEntriesDetailsService
    {
        private readonly ContabilidadContext _context;
        private readonly IMapper _mapper;

        public JournalEntriesDetailsService(ContabilidadContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<JournalEntryDetailDto>> CreateAsync(JournalEntryDetailCreateDto dto)
        {
            // Verificar si la partida contable existe
            var journalEntry = await _context.JournalEntries
                .FirstOrDefaultAsync(je => je.Id == dto.JournalEntryId);
            if (journalEntry == null)
            {
                return new ResponseDto<JournalEntryDetailDto>
                {
                    Message = "La partida contable no existe."
                };
            }

            // Verificar si la cuenta existe
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == dto.AccountNumber);
            if (account == null)
            {
                return new ResponseDto<JournalEntryDetailDto>
                {
                    Message = "El número de cuenta no existe."
                };
            }

            // Crear el detalle de la partida contable
            var entryDetail = new JournalEntryDetailEntity
            {
                Id = Guid.NewGuid(),
                JournalEntryId = dto.JournalEntryId,
                AccountNumber = dto.AccountNumber,
                Amount = dto.Amount,
            };

            _context.JournalEntryDetails.Add(entryDetail);
            await _context.SaveChangesAsync();

            // Obtener el saldo actual de la cuenta
            var existingBalance = await _context.Balances
                .FirstOrDefaultAsync(b => b.AccountNumber == dto.AccountNumber
                        && b.Year == journalEntry.Date.Year
                        && b.Month == journalEntry.Date.Month
                        && b.Day == journalEntry.Date.Day);

            decimal newTotalAmount;

            if (existingBalance != null)
            {
                // Si ya existe un balance, lo actualizamos
                newTotalAmount = existingBalance.Amount;

                if (account.TypeAccount == account.Name)
                {
                    newTotalAmount += dto.Amount; // Incrementar
                }
                else
                {
                    newTotalAmount -= dto.Amount; // Decrementar
                }

                // Actualizamos el balance existente
                existingBalance.Amount = newTotalAmount;
                _context.Balances.Update(existingBalance);
            }
            else
            {
                // Si no existe un balance para ese día, lo creamos
                newTotalAmount = (account.TypeAccount == account.Name) ? dto.Amount : -dto.Amount;

                var newBalance = new BalanceEntity
                {
                    JournalEntryId = dto.JournalEntryId,
                    AccountNumber = dto.AccountNumber,
                    Day = journalEntry.Date.Day,
                    Month = journalEntry.Date.Month,
                    Year = journalEntry.Date.Year,
                    Amount = newTotalAmount
                };

                _context.Balances.Add(newBalance);
            }

            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<JournalEntryDetailDto>(entryDetail);
            return new ResponseDto<JournalEntryDetailDto>
            {
                Message = "Detalle de partida y saldo actualizado correctamente.",
                Data = resultDto
            };
        }
    }
}
