using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Usuario
{
    public class UsuarioResponse : UsuarioBase
    {
        public int Id { get; set; }
    }
}