using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskFlow.Infrastructure.Database;
using TaskFlow.Infrastructure.Options;

namespace TaskFlow.Infrastructure.DI;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        services.AddDbContext<TaskFlowDbContext>((sp, options) =>
        {
            var dbOptions = sp.
                GetRequiredService<IOptions<DatabaseOptions>>()
                .Value;
            
            options.UseNpgsql(dbOptions.ConnectionString);
        });
        
        return services;
    }
}