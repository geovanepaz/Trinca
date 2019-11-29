using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Participante
{
    public class ParticipanteResponse : ParticipanteBase
    {
        public int Id { get; set; }
    }
}