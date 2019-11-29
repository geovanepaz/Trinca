using Core.ViewModels.Participante;

namespace Core.ViewModels.Evento
{
    public class ParticipanteEventoResponse : ParticipanteBase
    {
        public decimal Valor { get; set; }
        public decimal ValorSugerido { get; set; }
    }
}