using Core.Entities.Sql;
using Core.Interfaces.Repositories.Sql;
using Infra.Contexts;

namespace Infra.Repositories.Sql
{
    public class ParticipanteRepository : Repository<Participante>, IParticipanteRepository
    {
        public ParticipanteRepository(ITrincaContext context) : base(context)
        {
        }
    }
}