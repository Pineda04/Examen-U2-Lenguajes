using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Helpers;
using Examen2Lenguajes.API.Services;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examen2Lenguajes.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            // Add DbContext Contabilidad
            services.AddDbContext<ContabilidadContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DbContabilidad"))
            );

            // Add DbContext Logs
            services.AddDbContext<LogsContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DbLogs"))
            );
            
            // Add Custom Services
            services.AddTransient<IAuthService, AuthService>();
            
            // Add AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}