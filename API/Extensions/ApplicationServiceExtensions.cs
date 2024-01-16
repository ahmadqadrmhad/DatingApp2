using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration conig)
        {
            services.AddCors();
            services.AddScoped<ItokenService, TokenService>();
            

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(conig.GetConnectionString("DefaultConnection"));

            });

            return services;
        }

    }





}