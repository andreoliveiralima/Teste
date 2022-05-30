using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Teste_Domain.Interfaces;
using System.Text.Json;
using Serilog;
using RestSharp;
using Microsoft.Extensions.Logging;

namespace Teste_CrossCutting
{
    public class Empresa: IEmpresa
    {
        private string _baseUrl;
        public IHttpClientFactory _HttpClientFactory;
        private readonly ILogger<Empresa> _logger;
        public Empresa(IHttpClientFactory HttpClientFactory, ILogger<Empresa> logger)
        {
            IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _baseUrl = _config.GetSection("BaseUrl").Value;
            _HttpClientFactory = HttpClientFactory;
            _logger = logger;
        }
        public async Task<EmpresaResponse> GetEmpresa(int id)
        {
            try
            {
                _logger.LogInformation($"Solicitado dados da empresa: {id}...");
                var tsc = new TaskCompletionSource<IRestResponse<EmpresaResponse>>();

                var response = await CreateRetryPolicyAsync<EmpresaResponse>().ExecuteAsync(async () =>
                {
                    var client = new RestClient(_baseUrl + id);
                    var request = new RestRequest("", Method.GET);

                    client.ExecuteAsync<EmpresaResponse>(request, (res, handler) =>
                    {
                        tsc.SetResult(res);
                    });

                    return await tsc.Task;
                });

                if (response.Data!= null && !string.IsNullOrEmpty(response.Data.NomeEmpresa))
                {
                    _logger.LogInformation($"...Dados empresa: {response.Data.NomeEmpresa} localizados...");
                }
                else
                {
                    _logger.LogInformation($"...Dados empresa: {response.Data.NomeEmpresa} não localizados...");
                }
                
                return response.Data;


            }
            catch (HttpRequestException ex)
            {
                throw (ex);
            }
        }

        private static AsyncRetryPolicy<IRestResponse<T>> CreateRetryPolicyAsync<T>()
        {
            return Policy
                  .Handle<Exception>().Or<AggregateException>()
                  .OrResult<IRestResponse<T>>(r => (int)r.StatusCode == 500)
                  .WaitAndRetryAsync(5, times => TimeSpan.FromSeconds(2), onRetry: (exception, retryCount, context) =>
                  {
                      Log.Logger.Information($"Retry count {retryCount}. Exception: {exception}", retryCount, exception.Exception.Message);
                  });
        }

    }
}
