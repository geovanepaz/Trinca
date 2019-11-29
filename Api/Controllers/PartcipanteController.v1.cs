using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Interfaces;
using Core.ViewModels.Participante;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/participante")]
    public class PartcipanteController : Controller
    {
        private readonly IParticipanteAppService _appService;

        public PartcipanteController(IParticipanteAppService appService) => _appService = appService;

        [HttpGet]
        [ProducesResponseType(typeof(List<ParticipanteResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuscarTodos() => Ok(await _appService.BuscarTodos());

        [HttpPost]
        [ProducesResponseType(typeof(ParticipanteResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Adicionar([FromBody] ParticipanteRequest participante) => Created("", await _appService.Adicionar(participante));

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