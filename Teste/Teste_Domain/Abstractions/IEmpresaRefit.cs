using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teste_Domain.Entities;

namespace Teste_Domain.Abstractions
{
    public interface IEmpresaRefit
    {
        [Get("")]
        Task<EmpresaResponse> GetEmpresaRefit(int id);
    }
}
