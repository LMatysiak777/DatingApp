using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.interfaces;
using Microsoft.EntityFrameworkCore;
using API.Data; 
using API.Services;
using AutoMapper;
using API.Helpers;
namespace API.Extensions

{
    public static class ApplicationServiceExtensions

    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config) 
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof (AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options => 
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}