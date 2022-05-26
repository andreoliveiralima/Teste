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

        protected IEnumerable<T> Pesquisar<T>(string query)
        {
            IEnumerable<T> resultado = null;
            try
            {
                using (var conexao = ObterConexao())
                {
                    resultado = conexao.Query<T>(query);
                    conexao.Close();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected IEnumerable<T> Pesquisar<T>(string consulta, object parametros = null, CommandType tipoComando = CommandType.StoredProcedure, bool comBuffer = true)
        {
            IEnumerable<T> resultado = null;
            try
            {

                using (var conexao = ObterConexao())
                {
                    resultado = conexao.Query<T>(consulta, parametros, buffered: comBuffer, commandType: tipoComando);
                    conexao.Close();
                }
                return resultado;
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

        protected IEnumerable<T> Pesquisar<T>(Dictionary<string, object> query)
        {
            IEnumerable<T> resultado = null;
            try
            {
                using (var conexao = ObterConexao())
                {
                    foreach (KeyValuePair<string, object> t in query)
                    {
                        resultado = conexao.Query<T>(t.Key, t.Value);
                        conexao.Close();
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected string Executar(string query)
        {
            using (var conexao = ObterConexao())
            {
                var transaction = conexao.BeginTransaction();
                try
                {
                    conexao.Execute(query, transaction: transaction);
                    transaction.Commit();
                    conexao.Close();

                    return "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected string Executar(string query, object parametros)
        {
            using (var conexao = ObterConexao())
            {
                var transaction = conexao.BeginTransaction();
                try
                {
                    conexao.Execute(query, param: parametros, transaction: transaction);
                    transaction.Commit();
                    conexao.Close();

                    return "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected string Executar(string procedure, object parametros, CommandType tipoComando = CommandType.Text)
        {
            using (var conexao = ObterConexao())
            {
                var transaction = conexao.BeginTransaction();
                try
                {
                    conexao.Execute(procedure, param: parametros, transaction: transaction, commandType: tipoComando);
                    transaction.Commit();
                    conexao.Close();

                    return "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected string Executar(Dictionary<string, object> query)
        {
            using (var conexao = ObterConexao())
            {
                var transaction = conexao.BeginTransaction();
                try
                {
                    foreach (KeyValuePair<string, object> t in query)
                    {
                        conexao.Execute(t.Key, t.Value, transaction: transaction);
                    }
                    transaction.Commit();
                    conexao.Close();

                    return "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected string Executar(Dictionary<object, string> query)
        {
            using (var conexao = ObterConexao())
            {
                var transaction = conexao.BeginTransaction();
                try
                {
                    foreach (KeyValuePair<object, string> t in query)
                    {
                        conexao.Execute(t.Value, t.Key, transaction: transaction);
                    }
                    transaction.Commit();
                    conexao.Close();

                    return "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected IEnumerable<T> ExecutarComRetorno<T>(string query)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno;
                var transaction = conexao.BeginTransaction();
                try
                {
                    retorno = conexao.Query<T>(query, transaction: transaction);
                    transaction.Commit();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected IEnumerable<T> ExecutarComRetorno<T>(string query, object parametros)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno;
                var transaction = conexao.BeginTransaction();
                try
                {
                    retorno = conexao.Query<T>(query, param: parametros, transaction: transaction);
                    transaction.Commit();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected IEnumerable<T> ExecutarComRetorno<T>(string procedure, CommandType tipoComando = CommandType.Text)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno;
                var transaction = conexao.BeginTransaction();
                try
                {
                    retorno = conexao.Query<T>(procedure, null, transaction: transaction, commandType: tipoComando);
                    transaction.Commit();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected IEnumerable<T> ExecutarComRetorno<T>(string procedure, object parametros, CommandType tipoComando = CommandType.Text)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno;
                var transaction = conexao.BeginTransaction();
                try
                {
                    retorno = conexao.Query<T>(procedure, param: parametros, transaction: transaction, commandType: tipoComando);
                    transaction.Commit();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        protected IEnumerable<T> ExecutarComRetorno<T>(Dictionary<string, object> query)
        {
            using (var conexao = ObterConexao())
            {
                IEnumerable<T> retorno = null;
                var transaction = conexao.BeginTransaction();
                try
                {
                    foreach (KeyValuePair<string, object> t in query)
                    {
                        retorno = conexao.Query<T>(t.Key, t.Value, transaction: transaction);
                    }
                    transaction.Commit();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
