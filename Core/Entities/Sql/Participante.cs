using Core.Interfaces.Entities;

namespace Core.Entities.Sql
{
    public class Participante : EntityBase<int>
    {
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public string Email { get; set; }
    }
}