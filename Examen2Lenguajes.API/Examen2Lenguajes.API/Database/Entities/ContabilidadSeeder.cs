using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Examen2Lenguajes.API.Database.Entities
{
    public class ContabilidadSeeder
    {
        public static async Task LoadDataAsync(ContabilidadContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await LoadJournalEntries(context, loggerFactory);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ContabilidadSeeder>();
                logger.LogError(ex, "Error al iniciar la data del API");
            }
        }

        private static async Task LoadJournalEntries(ContabilidadContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var jsonFilePatch = "SeedData/entries.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePatch);
                var entries = JsonConvert.DeserializeObject<List<JournalEntryEntity>>(jsonContent);

                if (!await context.JournalEntries.AnyAsync())
                {
                    for (int i = 0; i < entries.Count; i++)
                    {
                        entries[i].CreatedBy = "2a373bd7-1829-4bb4-abb7-19da4257891d";
                        entries[i].CreatedDate = DateTime.Now;
                        entries[i].UpdatedBy = "2a373bd7-1829-4bb4-abb7-19da4257891d";
                        entries[i].UpdatedDate = DateTime.Now;
                    }
                    context.AddRange(entries);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ContabilidadSeeder>();
                logger.LogError(ex, "Error al ejecutar el seed de destinos");
            }
        }
    }
}