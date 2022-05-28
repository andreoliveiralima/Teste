using System.Diagnostics.CodeAnalysis;

namespace Teste_Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class PapelNegociadoResponse
    {
        public string Papel { get; set; }

        public string Empresa { get; set; }

        public decimal Valor { get; set; }
        public int idEmpresa { get; set; }
    }
}
