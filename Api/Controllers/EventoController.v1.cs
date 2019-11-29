using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Interfaces;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Evento")]
    public class EventoController : Controller
    {
        private readonly IEventoAppService _appService;

        public EventoController(IEventoAppService appservice) => _appService = appservice;

        [HttpGet]
        [ProducesResponseType(typeof(List<EventoResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuscarTodos() => Ok(await _appService.BuscarTodosEventos());

        [HttpGet]
        [Route("Detalhado/{id}")]
        [ProducesResponseType(typeof(List<EventoDetalhadoResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuscaDetalhada(int id) => Ok(await _appService.BuscaEventoDetalhado(id));

        [HttpPost]
        [ProducesResponseType(typeof(EventoResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Adicionar([FromBody] EventoRequest request) => Created("", await _appService.AdicionarEvento(request));

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Remover(int id)
        {
            await _appService.RemoverEvento(id);
            return NoContent();
        }

        [Route("participante")]
        [HttpPost]
        [ProducesResponseType(typeof(ValorParticipanteResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AdicionarParticipante([FromBody] ValorParticipanteRequest request) => Created("", await _appService.AdicionarParticipante(request));

        [Route("{id}/participante")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ValorParticipanteResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BuscarTodosParticipantes(int id) => Ok(await _appService.BuscarParticipantes(id));

        [Route("Participante")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoverParticipante(RemovePartcipanteEventoRequest request)
        {
             await _appService.RemoverParticipante(request);
            return NoContent();
        }

    }
}