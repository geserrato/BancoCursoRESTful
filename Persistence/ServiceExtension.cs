using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repository;

namespace Persistence;

public static class ServiceExtension
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
            sqlServerOptionsAction => sqlServerOptionsAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        #region Repositories

        services.AddTransient(typeof(IRepositoryAsync<>), typeof(MyRepositoryAsync<>));

        #endregion
        
        // #region Caching
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration.GetValue<string>("Caching:RedisConnection");
        // });
        // #endregion
    }
}