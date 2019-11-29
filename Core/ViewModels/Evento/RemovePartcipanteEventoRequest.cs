using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Evento
{
    public class RemovePartcipanteEventoRequest
    {
        public int IdEvento { get; set; }
        public int IdParticipante { get; set; }
    }
}