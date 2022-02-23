using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.interfaces;
using Microsoft.EntityFrameworkCore;
using API.Data; 
using API.Services;

namespace API.Extensions

{
    public static class ApplicationServiceExtensions

    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config) 
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options => 
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}