namespace API.Contexts
{
    public interface IMongoDBContext<TModel>
    {
        //Task<List<Models.CadPessoa>> GetPesquisaAsync(string nome, CancellationToken cancellationToken);

        #region CRUD
        Task<TModel> GetAsync(Guid id, CancellationToken cancellationToken);
        Task InsertAsync(TModel model, CancellationToken cancellationToken);
        Task UpdateAsync(TModel model, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        #endregion
    }
}
