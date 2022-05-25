using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teste_Domain.Entities;

namespace Teste_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PapelNegociadoController : ControllerBase
    {
        private readonly ILogger<PapelNegociadoController> _logger;

        public PapelNegociadoController(ILogger<PapelNegociadoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PapelNegociadoResponse> Get()
        {
            return null;
        }
    }
}
