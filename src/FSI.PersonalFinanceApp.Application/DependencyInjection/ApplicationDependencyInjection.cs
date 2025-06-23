using Microsoft.Extensions.DependencyInjection;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Services;

namespace FSI.PersonalFinanceApp.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<IExpenseAppService, ExpenseAppService>();
            services.AddScoped<IExpenseCategoryAppService, ExpenseCategoryAppService>();
            services.AddScoped<IIncomeAppService, IncomeAppService>();
            services.AddScoped<IIncomeCategoryAppService, IncomeCategoryAppService>();
            services.AddScoped<IFinancialGoalAppService, FinancialGoalAppService>();
            services.AddScoped<ITrafficAppService, TrafficAppService>();
            services.AddScoped<ITransactionAppService, TransactionAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IMessagingAppService, MessagingAppService>();
        }
    }
}
