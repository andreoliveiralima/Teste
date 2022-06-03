using System.Diagnostics.CodeAnalysis;
using Teste_Domain.Interfaces;

namespace Teste_Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class EmpresaResponse : IEmpresaResponse
    {
        public string NomeEmpresa { get; set; }
    }
}
