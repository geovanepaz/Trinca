using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class ParticipanteService : IParticipanteService
    {
        private readonly IParticipanteRepository _participante;

        public ParticipanteService(IParticipanteRepository participante) => _participante = participante;

        public async Task<Participante> Adicionar(Participante model)
        {
            try
            {
                return await _participante.InsertAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Adicionar ", e, model);
            }
        }

        public async Task Remover(Participante model)
        {
            try
            {
                await _participante.DeleteAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Remover ", e, model);
            }
        }
    }
}