﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System;
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
        private readonly IEmpresa _empresa;
        private readonly IServiceToken _serviceToken;

        public PapelNegociadoController(ILogger<PapelNegociadoController> logger, IPapelNegociado papelNegociado, IEmpresa empresa, IServiceToken serviceToken)
        {
            _logger = logger;
            _papelNegociacao = papelNegociado;
            _empresa = empresa;
            _serviceToken = serviceToken;
        }

        [HttpGet]
        [Route("get-valor-papel")]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PapelNegociadoResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PapelNegociadoRequest request)
        {
            try
            {
                _logger.LogInformation($"Solicitado dados para o papel: {request.Papel}");
                var retorno = await _papelNegociacao.GetPapelNegociado(request);
                if (retorno.Any())
                {
                    var empresa = await _empresa.GetEmpresa(retorno.FirstOrDefault().idEmpresa);
                    retorno.FirstOrDefault().Empresa = empresa?.NomeEmpresa;
                }
                _logger.LogInformation($"Dados do papel: {request.Papel} - Valor: {retorno.FirstOrDefault()?.Valor} - Empresa: {retorno.FirstOrDefault()?.Empresa}");
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}");
                return BadRequest("Erro Interno");
            }
            
        }

        [HttpGet]
        [Route("get-access-token")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PapelNegociadoResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                _logger.LogInformation($"Token solicitado...");
                var retorno = "Bearer " + await _serviceToken.GenerateToken();
                _logger.LogInformation($"...Token gerado com sucesso");
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}");
                return BadRequest("Erro Interno");
            }

        }
    }
}
