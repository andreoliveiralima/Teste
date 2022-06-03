using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Teste_Application.Controllers;
using Teste_Domain.Abstractions;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;
using Xunit;

namespace Test_API.Controller
{
    public class PapelNegociadoControllerTest
    {
        private readonly Mock<ILogger<PapelNegociadoController>> _mockLogger;
        private readonly Mock<IPapelNegociado> _mockPapelNegociacao;
        private readonly Mock<IEmpresa> _mockEmpresa;
        private readonly Mock<IServiceToken> _mockServiceToken;
        private readonly Mock<IEmpresaRefit> _mockEmpresaRefit;

        public PapelNegociadoControllerTest()
        {
            _mockLogger = new Mock<ILogger<PapelNegociadoController>>();
            _mockPapelNegociacao = new Mock<IPapelNegociado>();
            _mockEmpresa = new Mock<IEmpresa>();
            _mockServiceToken = new Mock<IServiceToken>();
            _mockEmpresaRefit = new Mock<IEmpresaRefit>();
        }

        [Theory(DisplayName = "Sucesso")]
        [InlineData("ITUB1")]
        [InlineData("ITUB2")]
        [InlineData("ITUB3")]
        [InlineData("ITUB4")]
        public async Task Get(string papel)
        {
            var papelNegociado = new PapelNegociadoRequest() { Papel = papel };

            var command = new PapelNegociadoController(_mockLogger.Object, _mockPapelNegociacao.Object, _mockEmpresa.Object, _mockServiceToken.Object, _mockEmpresaRefit.Object);
            var response = await command.Get(papelNegociado);

            Assert.Equal(HttpStatusCode.OK.GetHashCode(), ((ObjectResult)response).StatusCode);

        }

        [Theory(DisplayName = "Sucesso")]
        [InlineData("ITUB1")]
        [InlineData("ITUB2")]
        [InlineData("ITUB3")]
        [InlineData("ITUB4")]
        public async Task GetRefit(string papel)
        {
            var papelNegociado = new PapelNegociadoRequest() { Papel = papel };

            var command = new PapelNegociadoController(_mockLogger.Object, _mockPapelNegociacao.Object, _mockEmpresa.Object, _mockServiceToken.Object, _mockEmpresaRefit.Object);
            var response = await command.GetRefit(papelNegociado);

            Assert.Equal(HttpStatusCode.OK.GetHashCode(), ((ObjectResult)response).StatusCode);

        }

        [Fact(DisplayName = "Sucesso")]
        public async Task GetToken()
        {
            var command = new PapelNegociadoController(_mockLogger.Object, _mockPapelNegociacao.Object, _mockEmpresa.Object, _mockServiceToken.Object, _mockEmpresaRefit.Object);
            var response = await command.GetToken();

            Assert.Equal(HttpStatusCode.OK.GetHashCode(), ((ObjectResult)response).StatusCode);
        }
    }
}
