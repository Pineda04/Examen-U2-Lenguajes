using Examen2Lenguajes.API.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Examen2Lenguajes.API.Database.Entities
{
    public class ContabilidadSeeder
    {
        public static async Task LoadDataAsync(
            ContabilidadContext context,
            ILoggerFactory loggerFactory,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            try
            {
                await LoadRolesAndUsersAsync(userManager, roleManager, loggerFactory);
                //await LoadJournalEntries(context, loggerFactory);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ContabilidadSeeder>();
                logger.LogError(ex, "Error al iniciar la data del API");
            }
        }

        public static async Task LoadRolesAndUsersAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory
            ) 
        {
            try 
            {
                if (!await roleManager.Roles.AnyAsync()) 
                {
                    await roleManager.CreateAsync(new IdentityRole(RolesConstant.USER)); 
                }

                if (!await userManager.Users.AnyAsync()) 
                {
                    var normalUser = new IdentityUser
                    {
                        Email = "user@blogunah.edu",
                        UserName = "user@blogunah.edu",
                    };
                    
                    await userManager.CreateAsync(normalUser, "Temporal01*");

                    await userManager.AddToRoleAsync(normalUser, RolesConstant.USER);
                }
            } 
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ContabilidadContext>();
                logger.LogError(e.Message);
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
                    var user = await context.Users.FirstOrDefaultAsync();
                    for (int i = 0; i < entries.Count; i++)
                    {
                        entries[i].UserId = user.Id;
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