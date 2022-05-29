using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Teste_Application.Controllers;
using Teste_CrossCutting;
using Teste_Domain.Interfaces;
using Xunit;

namespace Test_API.Infra
{
    public class InfraTest
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Mock<ILogger<Empresa>> _mockLogger;
        private readonly IEmpresa _empresa;


        public InfraTest()
        {
            _mockLogger = new Mock<ILogger<Empresa>>();
            _empresa = new Empresa(_httpClientFactory, _mockLogger.Object);
        }

        [Theory(DisplayName = "Sucesso")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetEmpresa(int id)
        {
            var response = await _empresa.GetEmpresa(id);

            Assert.NotNull(response);

        }
    }
}
