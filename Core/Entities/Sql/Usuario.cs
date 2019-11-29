using Core.Interfaces.Entities;

namespace Core.Entities.Sql
{
    public class Usuario : EntityBase<int>
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}