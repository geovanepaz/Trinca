using AutoMapper;
using Core.Entities.Sql;
using Core.ViewModels.Participante;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;

namespace App.AutoMappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ValorParticipante, ValorParticipanteResponse>();
            CreateMap<Participante, ParticipanteResponse>();
            CreateMap<Evento, EventoDetalhadoResponse>();
            CreateMap<Evento, EventoResponse>()
                .ForMember(d => d.Observacao, o => o.MapFrom(s => s.Observacao != null ? s.Observacao : ""));
        }
    }
}