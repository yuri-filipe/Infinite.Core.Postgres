namespace Infinite.Core.Postgres.Repositories
{
    using Infinite.Core.Postgres.Mapping;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IReadOnlyRepository<T> where T : CoreEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<T> GetAsync(params Expression<Func<T, bool>>[] expressions);
        Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] expressions);
        Task<TDto> GetAsyncMapped<TDto>(params Expression<Func<T, bool>>[] expressions) where TDto : class;
        Task<IEnumerable<TDto>> FindAsyncMapped<TDto>(params Expression<Func<T, bool>>[] expressions) where TDto : class;
    }
}