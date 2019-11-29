using System;
using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Evento
{
    public class EventoBase
    {
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public DateTime DataEvento { get; set; }
    }
} 