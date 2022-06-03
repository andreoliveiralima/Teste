using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teste_Domain.Interfaces;
using Teste_Infra.Data.Context;

namespace Teste_Service.Configuration
{
    public class AcessoDadosConfig : IAcessoDadosConfig
    {
        public string MyConnection { get; set; }
    }
}
