namespace API.Contexts
{
    public interface IMongoDBContext
    {
        Task<bool> AnyAsync(string nome, CancellationToken cancellationToken);
        Task<List<Models.CadPessoa>> GetPesquisaAsync(string nome, CancellationToken cancellationToken);

        #region CRUD
        Task InsertAsync(Models.CadPessoa cadPessoa, CancellationToken cancellationToken);
        Task<Models.CadPessoa> GetAsync(string nome, CancellationToken cancellationToken);
        Task UpdateAsync(Models.CadPessoa cadPessoa, CancellationToken cancellationToken);
        Task DeleteAsync(string nome, CancellationToken cancellationToken);
        #endregion
    }
}
