using Core.Interfaces.Entities;
using System;

namespace Core.Entities.Sql
{
    public class ValorParticipante : EntityBase<int>
    {
        public int IdEvento { get; set; }
        public int IdParticipante { get; set; }
        public Decimal Valor { get; set; }
        public Decimal ValorSugerido { get; set; }
    }
}