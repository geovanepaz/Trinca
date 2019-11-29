using System.Threading.Tasks;
using Core.Entities.Sql;

namespace Core.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> Adicionar(Usuario dados);
        Task Remover(Usuario model);
    }
}