using System.Data;
using FSI.PersonalFinanceApp.Infrastructure.Context;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IDbContext _context;

        protected BaseRepository(IDbContext context)
        {
            _context = context;
        }

        protected IDbConnection CreateConnection()
        {
            return _context.CreateConnection();
        }
    }
}
