using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;

namespace Teste_Infra.Data.Repository
{
    public  class PapelNegociadoRepository: IPapelNegociado
    {
        private readonly IAcessoDados _iAcessoDados;

        public PapelNegociadoRepository(IAcessoDados iAcessoDados)
        {
            _iAcessoDados = iAcessoDados;
        }

        public IEnumerable<PapelNegociadoResponse> GetPapelNegociado(PapelNegociadoRequest papelNegociadoRequest)
        {
            try
            {
                var sql = @"select p.papel, vp.valor, p.idEmpresa
                            from papel p inner join valorpapel vp on p.id = vp.idpapel 
                            where p.papel = @PAPEL 
                            and vp.dtInsert = (select max(vp.dtInsert) from 
                            papel p inner join valorpapel vp on p.id = vp.idpapel 
                            where p.papel = @PAPEL);";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PAPEL", papelNegociadoRequest.Papel, DbType.String);

                var retorno = _iAcessoDados.Pesquisar<PapelNegociadoResponse>(sql, parameters);
                return retorno;
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }
    }
}
