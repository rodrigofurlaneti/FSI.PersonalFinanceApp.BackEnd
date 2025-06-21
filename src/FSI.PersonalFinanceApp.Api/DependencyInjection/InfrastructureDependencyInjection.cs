using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using FSI.PersonalFinanceApp.Infrastructure.Repositories;

namespace FSI.PersonalFinanceApp.Api.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbContext, DapperDbContext>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IIncomeCategoryRepository, IncomeCategoryRepository>();
            services.AddScoped<ITrafficRepository, TrafficRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
