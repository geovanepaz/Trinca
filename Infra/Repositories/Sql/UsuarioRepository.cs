using Core.Entities.Sql;
using Core.Interfaces.Repositories.Sql;
using Infra.Contexts;

namespace Infra.Repositories.Sql
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ITrincaContext context) : base(context)
        {
        }
    }
}