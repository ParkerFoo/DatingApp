using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static  class ApplicationServiceExtensions
    {

            public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
            {
                 //added by FRS for DB connection
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            ///

                 
             //added by FRS. we use AddScoped because IT WILL ONLY EXIST UNTIL the service has ended. there are others like AddSingleton and AddTransient
            services.AddScoped<ITokenService, TokenService>();

            return services;

            }
        
    }
}