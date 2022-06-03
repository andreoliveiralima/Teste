using Moq;
using System.Threading.Tasks;
using Teste_Domain.Entities;
using Teste_Domain.Interfaces;
using Teste_Infra.Data.Repository;
using Xunit;

namespace Test_API.Repository
{
    public class RepositoryTest
    {
        private readonly IPapelNegociado _papelNegociado;
        private readonly Mock<IAcessoDados> _acessoDadosMock;
        
        public RepositoryTest()
        {
            _acessoDadosMock = new Mock<IAcessoDados>();
            _papelNegociado = new PapelNegociadoRepository(_acessoDadosMock.Object);
        }

        [Theory(DisplayName = "Sucesso")]
        [InlineData("ITUB1")]
        [InlineData("ITUB2")]
        [InlineData("ITUB3")]
        [InlineData("ITUB4")]
        public async Task GetPapelNegociado(string papel)
        {
            var papelNegociado = new PapelNegociadoRequest() { Papel = papel };

            var result = _papelNegociado.GetPapelNegociado(papelNegociado);

            Assert.NotNull(result);

        }


    }
}
