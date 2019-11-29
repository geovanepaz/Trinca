using AutoMapper;
using Core.Entities.Sql;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;
using Core.ViewModels.Participante;
using Core.ViewModels.Usuario;

namespace App.AutoMappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ValorParticipanteRequest, ValorParticipante>();
            CreateMap<ParticipanteRequest, Participante>();
            CreateMap<UsuarioRequest, Usuario>();
            CreateMap<EventoRequest, Evento>();
        }
    }
}