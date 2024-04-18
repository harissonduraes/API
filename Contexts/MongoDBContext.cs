using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace API.Contexts
{
    public class MongoDBContext : IMongoDBContext
    {
        public MongoDBContext()
        {
            var connection = "mongodb://localhost:27017";
            MongoClient client = new (connection);

            _database = client.GetDatabase("API");
        }

        private IMongoDatabase _database;
        public IMongoCollection<TModel> GetCollection<TModel>(string name)
        {
            return _database.GetCollection<TModel>(name);
        }

        public async Task<bool> AnyAsync(string nome, CancellationToken cancellationToken)
        {
            return await GetCollection<Models.CadPessoa>("CadPessoa")
                .AsQueryable()
                .Where(w => w.Nome == nome)
                .AnyAsync(cancellationToken);
        }

        public async Task<List<Models.CadPessoa>> GetPesquisaAsync(string nome, CancellationToken cancellationToken)
        {
            return await GetCollection<Models.CadPessoa>("CadPessoa")
                .AsQueryable()
                .Where(w =>w.Nome == nome)
                .ToListAsync(cancellationToken);
        }

        #region CRUD
        public async Task InsertAsync(Models.CadPessoa cadPessoa, CancellationToken cancellationToken)
        {
            await GetCollection<Models.CadPessoa>("CadPessoa")
                .InsertOneAsync(cadPessoa, cancellationToken : cancellationToken);
        }

        public async Task<Models.CadPessoa> GetAsync(string nome, CancellationToken cancellationToken)
        {
            return await GetCollection<Models.CadPessoa>("CadPessoa")
                .AsQueryable()
                .Where(w => w.Nome == nome)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(Models.CadPessoa cadPessoa, CancellationToken cancellationToken)
        {
            await GetCollection<Models.CadPessoa>("CadPessoa")
                .ReplaceOneAsync(w => w.Nome ==  cadPessoa.Nome, cadPessoa, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string nome, CancellationToken cancellationToken)
        {
            await GetCollection<Models.CadPessoa>("CadPessoa")
                .DeleteOneAsync(w => w.Nome == nome, cancellationToken);
        }
        #endregion
    }
}
