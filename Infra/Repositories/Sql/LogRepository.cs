using Core.Entities.Sql;
using Core.Interfaces.Repositories.Sql;
using Infra.Contexts;

namespace Infra.Repositories.Sql
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(ITrincaLogContext context) : base(context)
        {
        }
    }
}