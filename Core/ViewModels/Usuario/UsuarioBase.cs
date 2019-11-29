using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Usuario
{
    public class UsuarioBase
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}