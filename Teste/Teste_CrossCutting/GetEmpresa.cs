using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Teste_CrossCutting
{
    public class Empresa
    {
        public async Task<string> GetEmpresa(string papel)
        {
            AsyncRetryPolicy<HttpResponseMessage> httpRetryPolicy = Policy
                .HandleResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(10));

            HttpResponseMessage httpResponseMessage = await httpRetryPolicy.ExecuteAsync(() => HttpClient.GetAsync(""));
            return httpResponseMessage.RequestMessage.ToString();
        }
    }
}
