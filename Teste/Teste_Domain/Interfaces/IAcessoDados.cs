using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Teste_Domain.Interfaces
{
    public interface IAcessoDados
    {
        public IDbConnection ObterConexao();
        IEnumerable<T> Pesquisar<T>(string query, object parametros);

    }
}
