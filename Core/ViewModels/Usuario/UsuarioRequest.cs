using System.Net;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Core.ViewModels.Usuario
{
    public class UsuarioRequest : UsuarioBase
    {
        public string Senha { get; set; }
    }
}