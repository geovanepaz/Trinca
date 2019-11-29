using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.Participante;
using Core.ViewModels.Usuario;

namespace App.Interfaces
{
    public interface IUsuarioAppService
    {
        Task<List<UsuarioResponse>> BuscarTodos();
        Task<UsuarioResponse> Adicionar(UsuarioRequest modelo);
        Task Remover(int id);
    }
}