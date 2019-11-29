using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;
using Core.ViewModels.Participante;

namespace App.Interfaces
{
    public interface IEventoAppService
    {
        Task<List<EventoResponse>> BuscarTodosEventos();
        Task<EventoDetalhadoResponse> BuscaEventoDetalhado(int idEvento);
        Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento);
        Task<EventoResponse> AdicionarEvento(EventoRequest request);
        Task<ValorParticipanteResponse> AdicionarParticipante(ValorParticipanteRequest request);
        Task RemoverEvento(int id);
        Task RemoverParticipante(RemovePartcipanteEventoRequest model);
    }
}