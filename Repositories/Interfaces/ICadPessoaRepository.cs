using API.Contexts;

namespace API.Repositories.Interfaces
{
    public interface ICadPessoaRepository : IMongoDBContext<Models.CadPessoa>
    {
        Task<List<Models.CadPessoa>> GetBySearchAsync(string value, CancellationToken cancellationToken);
    }
}
