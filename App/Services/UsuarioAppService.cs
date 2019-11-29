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
using Core.ViewModels.Usuario;
using Cross.Util.Extensions;

namespace App.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioAppService(IUsuarioRepository usuarioRepo, IMapper mapper, IUsuarioService usuarioService)
        {
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        public async Task<UsuarioResponse> Adicionar(UsuarioRequest request)
        {
            var model = request.MapTo<Usuario>(_mapper);
            var responseModel = await _usuarioService.Adicionar(model);

            return responseModel.MapTo<UsuarioResponse>(_mapper);
        }

        public async Task<List<UsuarioResponse>> BuscarTodos()
        {
            var participante = await _usuarioRepo.AllAsync();

            return participante.MapTo<List<UsuarioResponse>>(_mapper);
        }

        public async Task Remover(int id)
        {
            var model = _usuarioRepo.GetById(id);

            if (model == null)
            {
                throw new NotFoundException("App.Services.ParticipanteAppService Remover", id);
            }

            await _usuarioService.Remover(model);
        }
    }
}