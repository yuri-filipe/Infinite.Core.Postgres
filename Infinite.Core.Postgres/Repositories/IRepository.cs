namespace Infinite.Core.Postgres.Repositories
{
    using Infinite.Core.Postgres.Mapping;
    using System.Linq.Expressions;

    public interface IRepository<T> where T : CoreEntity<long>
    {
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(List<Expression<Func<T, bool>>> expressions);
        Task<IEnumerable<TDto>> GetAsync<TDto>(List<Expression<Func<T, bool>>> expressions) where TDto : class;
        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities, int batchSize = 100);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IEnumerable<T> entities, int batchSize = 100);
        Task DeleteAsync(T entity);
    }
}
