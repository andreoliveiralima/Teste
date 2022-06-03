using System.Collections.Generic;
using Teste_Domain.Entities;

namespace Teste_Domain.Interfaces
{
    public interface IPapelNegociado
    {
        IEnumerable<PapelNegociadoResponse> GetPapelNegociado(PapelNegociadoRequest papelNegociadoRequest);
    }
}
