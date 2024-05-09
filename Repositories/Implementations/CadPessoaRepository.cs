using API.Contexts;
using API.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace API.Repositories.Implementations
{
    public class CadPessoaRepository : MongoDBContext<Models.CadPessoa>, ICadPessoaRepository
    {
        public CadPessoaRepository()
        {
        }

        public async Task<List<Models.CadPessoa>> GetBySearchAsync(string value, CancellationToken cancellationToken)
        {
            return await Query
                .Where(w => w.Nome.StartsWith(value))
                .ToListAsync(cancellationToken);
        }
    }
}
