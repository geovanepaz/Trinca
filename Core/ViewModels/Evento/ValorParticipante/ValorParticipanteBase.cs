using System;
using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Evento.ValorParticipante
{
    public class ValorParticipanteBase
    {
        public int IdEvento { get; set; }
        public int IdParticipante { get; set; }
        public Decimal Valor { get; set; }
        public Decimal ValorSugerido { get; set; }
    }
} 