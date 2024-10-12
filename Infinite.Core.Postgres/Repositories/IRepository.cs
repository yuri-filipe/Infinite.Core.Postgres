namespace Infinite.Core.Postgres.Repositories
{
    using Infinite.Core.Postgres.Mapping;
    public interface IRepository<T> : IReadOnlyRepository<T> where T : CoreEntity<long>
    {
        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities, int batchSize = 100);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IEnumerable<T> entities, int batchSize = 100);
        Task DeleteAsync(T entity);
    }
}
