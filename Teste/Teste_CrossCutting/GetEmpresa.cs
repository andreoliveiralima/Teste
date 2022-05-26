using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Teste_Domain.Interfaces;
using System.Text.Json;

namespace Teste_CrossCutting
{
    public class Empresa: IEmpresa
    {
        private string _baseUrl;
        public IHttpClientFactory _HttpClientFactory;
        private readonly AsyncRetryPolicy _retryPolicy;
        public Empresa(IHttpClientFactory HttpClientFactory)
        {
            IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _baseUrl = _config.GetSection("BaseUrl").Value;
            _retryPolicy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(5, times => TimeSpan.FromSeconds(10));
            _HttpClientFactory = HttpClientFactory;
        }
        public async Task<EmpresaResponse> GetEmpresa(int id)
        {
            try
            {
                var empresaResponse = new EmpresaResponse();
                var httpClient = _HttpClientFactory.CreateClient("MyHttpClient");
                httpClient.BaseAddress = new Uri(_baseUrl + id);
                int a = 0;
                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    HttpResponseMessage response = await httpClient.GetAsync("");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //throw (new Exception(response.StatusCode.ToString()));
                        a++;
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var nomeEmpresa = JsonSerializer.Deserialize<EmpresaResponse>(content);
                        empresaResponse.NomeEmpresa = nomeEmpresa.title;
                        return empresaResponse;
                    }
                });

            }
            catch (HttpRequestException ex)
            {
                throw (ex);
            }
        }

        
    }
}
