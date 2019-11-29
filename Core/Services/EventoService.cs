using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _evento;

        public EventoService(IEventoRepository evento) => _evento = evento;

        public async Task<Evento> Adicionar(Evento model)
        {
            try
            {
                return await _evento.InsertAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Adicionar ", e, model);
            }
        }

        public async Task Remover(Evento model)
        {
            try
            {
                await _evento.DeleteAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Remover ", e, model);
            }
        }
    }
}