using Core.Interfaces.Entities;
using System;

namespace Core.Entities.Sql
{
    public class Evento : EntityBase<int>
    {
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public DateTime DataEvento { get; set; }
    }
}