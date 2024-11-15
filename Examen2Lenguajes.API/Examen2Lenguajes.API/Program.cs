using Examen2Lenguajes.API;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Database.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

// Configuración para cargar el seed de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ContabilidadContext>();
        
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await ContabilidadSeeder.LoadDataAsync(context, loggerFactory, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error al ejecutar el seed de datos");
    }
}

app.Run();