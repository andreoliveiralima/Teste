using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Teste_Domain.Abstractions;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;

namespace Teste_CrossCutting
{
    public class EmpresaRefit: IEmpresaRefit
    {
        
        private readonly ILogger<Empresa> _logger;
        private readonly IApiConfig _apiConfig;
        public EmpresaRefit(ILogger<Empresa> logger, IApiConfig apiConfig)
        {
            _logger = logger;
            _apiConfig = apiConfig;
        }
        
        public async Task<EmpresaResponse> GetEmpresaRefit(int id)
        {
            try
            {
                var api = RestService
                    .For<IEmpresaRefit>($"{_apiConfig.BaseUrl}");

                return await api.GetEmpresaRefit(id);

            }
            catch (HttpRequestException ex)
            {
                throw (ex);
            }
        }
    }
}
