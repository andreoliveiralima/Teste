using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;

namespace Teste_CrossCutting
{
    public class Empresa: IEmpresa
    {
        
        private readonly ILogger<Empresa> _logger;
        private readonly IApiConfig _apiConfig;
        private readonly HttpClient _httpClient;
        public Empresa(ILogger<Empresa> logger, IApiConfig apiConfig, HttpClient httpClient)
        {
            _logger = logger;
            _apiConfig = apiConfig;
            _httpClient = httpClient;
        }
        
        public async Task<EmpresaResponse> GetEmpresa(int id)
        {
            try
            {
                _logger.LogInformation($"Solicitado dados da empresa: {id}...");
                var retorno = await _httpClient.GetFromJsonAsync<EmpresaResponse>(
                    $"{_apiConfig.BaseUrl + "?Id=" + id}");

                return retorno;
                
            }
            catch (HttpRequestException ex)
            {
                throw (ex);
            }
        }
    }
}
