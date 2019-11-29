using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuario;

        public UsuarioService(IUsuarioRepository usuario) => _usuario = usuario;

        public async Task<Usuario> Adicionar(Usuario model)
        {
            try
            {
                return await _usuario.InsertAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Adicionar ", e, model);
            }
        }

        public async Task Remover(Usuario model)
        {
            try
            {
                await _usuario.DeleteAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Remover ", e, model);
            }
        }
    }
}