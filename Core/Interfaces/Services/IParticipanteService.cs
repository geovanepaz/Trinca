using System.Threading.Tasks;
using Core.Entities.Sql;

namespace Core.Interfaces.Services
{
    public interface IParticipanteService
    {
        Task<Participante> Adicionar(Participante dados);
        Task Remover(Participante model);
    }
}