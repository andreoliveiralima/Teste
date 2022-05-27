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
            _HttpClientFactory = HttpClientFactory;
        }
        public async Task<EmpresaResponse> GetEmpresa(int id)
        {
            try
            {
                var tsc = new TaskCompletionSource<IRestResponse<EmpresaResponse>>();

                var response = await CreateRetryPolicyAsync<EmpresaResponse>().ExecuteAsync(async () =>
                {
                    var client = new RestClient(_baseUrl + id);
                    var request = new RestRequest("", Method.POST);

                    client.ExecuteAsync<EmpresaResponse>(request, (res, handler) =>
                    {
                        tsc.SetResult(res);
                    });

                    return await tsc.Task;
                });

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
                  .WaitAndRetryAsync(5, times => TimeSpan.FromSeconds(1), onRetry: (exception, retryCount, context) =>
                  {
                      Log.Logger.Information($"Retry count {retryCount}. Exception: {exception}", retryCount, exception.Exception.Message);
                  });
        }

    }
}
