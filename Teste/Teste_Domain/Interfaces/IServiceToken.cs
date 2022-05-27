using System.Threading.Tasks;

namespace Teste_Domain.Interfaces
{
    public interface IServiceToken
    {
        Task<string> GenerateToken();
    }
}
