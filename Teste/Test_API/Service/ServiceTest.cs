using System.Threading.Tasks;
using Teste_Domain.Interfaces;
using Teste_Service.Services;
using Xunit;

namespace Test_API.Service
{
    public class ServiceTest
    {
        private readonly IServiceToken _serviceToken;
        
        public ServiceTest()
        {
            _serviceToken = new ServiceToken();
        }

        [Fact(DisplayName = "Sucesso")]
        public async Task GenerateToken()
        {
            var response = await _serviceToken.GenerateToken();

            Assert.NotNull(response);
            Assert.NotEmpty(response);
        }
    }
}
