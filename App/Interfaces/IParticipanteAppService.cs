using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.Participante;

namespace App.Interfaces
{
    public interface IParticipanteAppService
    {
        Task<List<ParticipanteResponse>> BuscarTodos();
        Task<ParticipanteResponse> Adicionar(ParticipanteRequest modelo);
        Task Remover(int id);
    }
}