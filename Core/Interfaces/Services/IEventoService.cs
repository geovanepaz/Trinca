using System.Threading.Tasks;
using Core.Entities.Sql;

namespace Core.Interfaces.Services
{
    public interface IEventoService
    {
        Task<Evento> Adicionar(Evento model);
        Task Remover(Evento model);
    }
}