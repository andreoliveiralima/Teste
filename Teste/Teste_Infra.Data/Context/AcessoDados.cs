using Dapper;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Teste_Domain.Interfaces;

namespace Teste_Infra.Data.Context
{
    public class AcessoDados : IAcessoDados
    {
        private readonly IAcessoDadosConfig _iAcessoDadosConfig;

        public AcessoDados(IAcessoDadosConfig iAcessoDadosConfig)
        {
            _iAcessoDadosConfig = iAcessoDadosConfig;
        }

        public IDbConnection ObterConexao()
        {
            try
            {
                IDbConnection conexao = null;
                {
                    conexao = new SqlConnection(_iAcessoDadosConfig.MyConnection);
                }
                conexao.Open();
                return conexao;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public IEnumerable<T> Pesquisar<T>(string query, object parametros)
        {
            var conexao = ObterConexao();
            IEnumerable<T> retorno;

            try
            {
                retorno = conexao.Query<T>(query, param: parametros);
                conexao.Close();

                return retorno;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
    }
}
