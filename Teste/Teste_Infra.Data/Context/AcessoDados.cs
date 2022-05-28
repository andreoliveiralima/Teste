using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Teste_Infra.Data.Context
{
    public class AcessoDados
    {
        private IDbConnection ObterConexao()
        {
            IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            try
            {
                IDbConnection conexao = null;
                {
                    conexao = new SqlConnection(_config.GetConnectionString("MyConnection"));
                }
                conexao.Open();
                return conexao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task<IEnumerable<T>> Pesquisar<T>(string query, object parametros)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno;
                try
                {
                    retorno = await conexao.QueryAsync<T>(query, param: parametros);
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
