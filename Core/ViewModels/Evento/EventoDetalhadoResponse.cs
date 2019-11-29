using System.Collections.Generic;
using System.Linq;

namespace Core.ViewModels.Evento
{
    public class EventoDetalhadoResponse : EventoBase 
    {
        public int Id { get; set; }
        public List<ParticipanteEventoResponse> Participantes { get; set; }
        public decimal ValorArrecadado
        {
            get
            {
                return Participantes.Sum(x => x.Valor);
            }
        }

        public int TotalParticipante
        {
            get
            {
                return Participantes.Count();
            }
        }
    }
}