using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure.Database;
using TaskFlow.Infrastructure.DI;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.AddDatabase(configuration);

await using var serviceProvider = services.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();

var dbContext = scope.ServiceProvider
    .GetRequiredService<TaskFlowDbContext>();

await dbContext.Database.MigrateAsync();

Console.WriteLine("Database migrations applied successfully.");