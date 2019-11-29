using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.ViewModels.Evento;

namespace Core.Interfaces.Services
{
    public interface IEventoParticipante 
    {
        Task<ValorParticipante> Adicionar(ValorParticipante model);
        Task Remover(ValorParticipante model);
        Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento);
        Task RemoverParticipante(RemovePartcipanteEventoRequest request);
    }
}