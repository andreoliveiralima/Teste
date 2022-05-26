using Dapper;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;
using Teste_Infra.Data.Context;

namespace Teste_Infra.Data.Repository
{
    public  class PapelNegociadoRepository : AcessoDados, IPapelNegociado
    {
        public async Task<IEnumerable<PapelNegociadoResponse>> GetPapelNegociado(PapelNegociadoRequest papelNegociadoRequest)
        {
            try
            {
                var sql = @"select p.papel, vp.valor
                            from papel p inner join valorpapel vp on p.id = vp.idpapel 
                            where p.papel = @PAPEL 
                            and vp.dtInsert = (select max(vp.dtInsert) from 
                            papel p inner join valorpapel vp on p.id = vp.idpapel 
                            where p.papel = @PAPEL)
                            group by p.papel, vp.valor;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PAPEL", papelNegociadoRequest.Papel, DbType.String);
                var retorno = await Pesquisar<PapelNegociadoResponse>(sql, parameters);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposedValue = true;
        }


        ~PapelNegociadoRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
