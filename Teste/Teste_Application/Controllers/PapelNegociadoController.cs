using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;

namespace Teste_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PapelNegociadoController : ControllerBase
    {
        private readonly ILogger<PapelNegociadoController> _logger;
        private readonly IPapelNegociado _papelNegociacao;

        public PapelNegociadoController(ILogger<PapelNegociadoController> logger, IPapelNegociado papelNegociado)
        {
            _logger = logger;
            _papelNegociacao = papelNegociado;
        }

        [HttpGet]
        [Route("get-valor-papel")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PapelNegociadoResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PapelNegociadoRequest request)
        {
            var retorno = await _papelNegociacao.GetPapelNegociado(request);
            return Ok(retorno);
        }
    }
}
