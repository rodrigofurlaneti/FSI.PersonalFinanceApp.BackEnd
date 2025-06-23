using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // <-- esta linha faltava
using FSI.PersonalFinanceApp.Application.Messaging;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using FSI.PersonalFinanceApp.Infrastructure.Messaging;
using FSI.PersonalFinanceApp.Infrastructure.Repositories;

namespace FSI.PersonalFinanceApp.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbContext, DapperDbContext>();

            // Repositórios
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IIncomeCategoryRepository, IncomeCategoryRepository>();
            services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
            services.AddScoped<ITrafficRepository, TrafficRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessagingRepository, MessagingRepository>();

            // RabbitMQ Publisher
            services.AddSingleton<IMessageQueuePublisher, RabbitMqPublisher>();
        }
    }
}
