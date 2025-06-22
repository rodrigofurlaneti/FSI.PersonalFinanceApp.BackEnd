using Microsoft.Extensions.Hosting;
using FSI.PersonalFinanceApp.Worker;
using FSI.PersonalFinanceApp.Infrastructure.DependencyInjection;
using FSI.PersonalFinanceApp.Application.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ExpenseCategoryConsumer>();
        services.AddApplicationServices(); // camada Application
        services.AddInfrastructure(context.Configuration); // camada Infrastructure
    })
    .Build();

host.Run();
