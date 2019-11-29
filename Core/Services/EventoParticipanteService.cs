using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;
using Core.ViewModels.Evento;

namespace Core.Services
{
    public class EventoParticipanteService : IEventoParticipante
    {
        private readonly IValorParticipanteRepository _participante;

        public EventoParticipanteService(IValorParticipanteRepository participante) => _participante = participante;

        public async Task<ValorParticipante> Adicionar(ValorParticipante model)
        {
            try
            {
                return await _participante.InsertAsync(model);
            }
            catch (SqlException e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Adicionar ", e, model);
            }
            catch (Exception e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Adicionar ", e, model);
            }
        }

        public async Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento)
        {
            return await _participante.BuscarParticipantes(idEvento);
        }

        public async Task RemoverParticipante(RemovePartcipanteEventoRequest request)
        {
            try
            {
                await _participante.RemoveParticipante(request);
            }
            catch (Exception e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService RemoverParticipante ", e, request);
            }
        }

        public async Task Remover(ValorParticipante model)
        {
            try
            {
                await _participante.DeleteAsync(model);
            }
            catch (Exception e)
            {
                throw new InternalErrorException("Core.Services.ParticipanteService Remover ", e, model);
            }
        }
    }
}