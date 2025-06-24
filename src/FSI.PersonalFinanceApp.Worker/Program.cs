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
        services.AddHostedService<AccountConsumer>();
        services.AddHostedService<ExpenseCategoryConsumer>();
        services.AddHostedService<ExpenseConsumer>();
        services.AddHostedService<FinancialGoalConsumer>();
        services.AddHostedService<IncomeConsumer>();
        services.AddHostedService<IncomeCategoryConsumer>();
        services.AddHostedService<TrafficConsumer>();
        services.AddHostedService<TransactionConsumer>();
        services.AddHostedService<UserConsumer>();
        services.AddApplicationServices(); // camada Application
        services.AddInfrastructure(context.Configuration); // camada Infrastructure
    })
    .Build();

host.Run();
