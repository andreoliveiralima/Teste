using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //[Route("{papel:string}")]
        public async Task<IActionResult> Get([FromBody] PapelNegociadoRequest request)
        {
            //PapelNegociadoRequest pNr = new PapelNegociadoRequest();
            //pNr.Papel = papel;
            var retorno = _papelNegociacao.GetPapelNegociado(request);
            return Ok(retorno);
        }

        //[HttpGet]
        //public IActionResult Get([FromBody] PapelNegociadoRequest papelRequest)
        //{
        //    var retorno = _papelNegociacao.GetPapelNegociado(papelRequest);
        //    return Ok(retorno);
        //}
    }
}
