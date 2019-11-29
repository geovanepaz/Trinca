using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Interfaces;
using Core.ViewModels.Usuario;
using Microsoft.AspNetCore.Mvc;
using UsuarioResponse = Core.ViewModels.Usuario.UsuarioResponse;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuario")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioAppService _appService;

        public UsuarioController(IUsuarioAppService appService) => _appService = appService;

        [HttpGet]
        [ProducesResponseType(typeof(List<UsuarioResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuscarTodos() => Ok(await _appService.BuscarTodos());

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Adicionar([FromBody] UsuarioRequest usuario) => Created("", await _appService.Adicionar(usuario));

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Remover(int id)
        {
            await _appService.Remover(id);
            return NoContent();
        }
    }
}