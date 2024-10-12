namespace Infinite.Core.Postgres.Repositories
{
    using System.Linq.Expressions;
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(List<Expression<Func<T, bool>>> expressions);
        Task<IEnumerable<TDto>> GetAsync<TDto>(List<Expression<Func<T, bool>>> expressions) where TDto : class;
    }
}
