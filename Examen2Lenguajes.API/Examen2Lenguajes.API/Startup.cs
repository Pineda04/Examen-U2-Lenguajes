using System.Text;
using Examen2Lenguajes.API.Database;
using Examen2Lenguajes.API.Helpers;
using Examen2Lenguajes.API.Services;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            services.AddHttpContextAccessor();

            var contabilidad = Configuration.GetConnectionString("DbContabilidad");
            var logs = Configuration.GetConnectionString("DbLogs");

            // Add DbContext Contabilidad
            services.AddDbContext<ContabilidadContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DbContabilidad"))
            );

            // Add DbContext Logs
            //services.AddDbContext<LogsContext>(options => options.UseSqlServer(
                //Configuration.GetConnectionString("DbLogs"))
            //);
            
            // Add Custom Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountsService, AccountsService>();
            services.AddTransient<IBalancesService, BalancesService>();
            services.AddTransient<IAuditService, AuditService>();

            // Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options => 
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ContabilidadContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters 
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            
            // Add AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // CORS Configuration
            services.AddCors(opt => 
            {
                var allowURLS = Configuration.GetSection("AllowURLS").Get<string[]>();
                
                opt.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins(allowURLS)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
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

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}