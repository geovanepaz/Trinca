using Core.Entities.Sql;
using Core.Interfaces.Repositories.Sql;
using Infra.Contexts;

namespace Infra.Repositories.Sql
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        public EventoRepository(ITrincaContext context) : base(context)
        {
        }
    }
}