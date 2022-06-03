using Teste_Domain.Interfaces;

namespace Teste_Service.Configuration
{
    public class ApiConfig : IApiConfig
    {
        public string BaseUrl { get; set; }
    }
}
