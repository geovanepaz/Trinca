using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Participante
{
    public class ParticipanteBase
    {
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public string Email { get; set; }
    }
}