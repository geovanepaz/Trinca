using Core.Entities.Sql;
using Core.ViewModels.Evento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories.Sql
{
    public interface IValorParticipanteRepository : IRepository<ValorParticipante>
    {
        Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento);
        Task RemoveParticipante(RemovePartcipanteEventoRequest request);
    }
}