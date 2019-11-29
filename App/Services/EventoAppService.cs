using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Interfaces;
using AutoMapper;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;
using Cross.Util.Extensions;

namespace App.Services
{
    public class EventoAppService : IEventoAppService
    {
        private readonly IEventoRepository _eventocoRepo;
        private readonly IEventoService _eventoService;
        private readonly IEventoParticipante _eventoParticipanteService;
        private readonly IMapper _mapper;

        public EventoAppService(IEventoRepository eventoRepo, IMapper mapper, IEventoService eventoService, IEventoParticipante eventoParticipanteService)
        {
            _eventocoRepo = eventoRepo;
            _mapper = mapper;
            _eventoService = eventoService;
            _eventoParticipanteService = eventoParticipanteService;
        }

        public async Task<EventoResponse> AdicionarEvento(EventoRequest request)
        {
            var model = request.MapTo<Evento>(_mapper);
            var responseModel = await _eventoService.Adicionar(model);

            return responseModel.MapTo<EventoResponse>(_mapper);
        }

        public async Task<ValorParticipanteResponse> AdicionarParticipante(ValorParticipanteRequest request)
        {
            var model = request.MapTo<ValorParticipante>(_mapper);
            var responseModel = await _eventoParticipanteService.Adicionar(model);

            return responseModel.MapTo<ValorParticipanteResponse>(_mapper);
        }

        public async Task<EventoDetalhadoResponse> BuscaEventoDetalhado(int idEvento)
        {
            var evento = _eventocoRepo.Find(x => x.Id == idEvento).FirstOrDefault();

            if (evento == null)
            {
                throw new NotFoundException("App.Services.EventoAppService BuscaEventoDetalhado", idEvento);
            }
            
            var eventoResponse = evento.MapTo<EventoDetalhadoResponse>(_mapper);

            eventoResponse.Participantes = await _eventoParticipanteService.BuscarParticipantes(idEvento);

            return eventoResponse;
        }

        public async Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento)
        {
            return await _eventoParticipanteService.BuscarParticipantes(idEvento);
        }

        public async Task<List<EventoResponse>> BuscarTodosEventos()
        {
            var retorno = await _eventocoRepo.AllAsync();

            return retorno.MapTo<List<EventoResponse>>(_mapper);
        }

        public async Task RemoverEvento(int id)
        {
            var model = _eventocoRepo.GetById(id);

            if (model == null)
            {
                throw new NotFoundException("App.Services.ParticipanteAppService Remover", id);
            }

            await _eventoService.Remover(model);
        }

        public async Task RemoverParticipante(RemovePartcipanteEventoRequest request)
        {
            await _eventoParticipanteService.RemoverParticipante(request);
        }
    }
}