using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Interfaces;
using AutoMapper;
using Core.Entities.Sql;
using Core.Exceptions;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;
using Core.ViewModels.Participante;
using Cross.Util.Extensions;

namespace App.Services
{
    public class ParticipanteAppService : IParticipanteAppService
    {
        private readonly IParticipanteRepository _participanteRepo;
        private readonly IParticipanteService _participanteService;
        private readonly IMapper _mapper;

        public ParticipanteAppService(IParticipanteRepository participanteRepo, IMapper mapper, IParticipanteService participanteService)
        {
            _participanteRepo = participanteRepo;
            _mapper = mapper;
            _participanteService = participanteService;
        }

        public async Task<ParticipanteResponse> Adicionar(ParticipanteRequest request)
        {
            Participante model = request.MapTo<Participante>(_mapper);
            Participante responseModel = await _participanteService.Adicionar(model);

            return responseModel.MapTo<ParticipanteResponse>(_mapper);
        }

        public async Task<List<ParticipanteResponse>> BuscarTodos()
        {
            var participante = await _participanteRepo.AllAsync();

            return participante.MapTo<List<ParticipanteResponse>>(_mapper);
        }

        public async Task Remover(int id)
        {
            var model = _participanteRepo.GetById(id);

            if (model == null)
            {
                throw new NotFoundException("App.Services.ParticipanteAppService Remover", id);
            }

            await _participanteService.Remover(model);
        }
    }
}