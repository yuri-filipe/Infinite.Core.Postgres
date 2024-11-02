namespace Infinite.Core.Postgres.Repositories
{
    using AutoMapper;
    using Infinite.Core.Postgres.Context;
    using Infinite.Core.Postgres.Mapping;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : CoreEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public ReadOnlyRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _mapper = mapper;
        }

        private IQueryable<T> BuildQuery(params Expression<Func<T, bool>>[] expressions)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().Where(x => !x.Excluido);

            if (expressions != null)
            {
                foreach (var expression in expressions)
                {
                    query = query.Where(expression);
                }
            }

            return query;
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && !e.Excluido);
        }

        public virtual async Task<T> GetAsync(params Expression<Func<T, bool>>[] expressions)
        {
            var query = BuildQuery(expressions);
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] expressions)
        {
            var query = BuildQuery(expressions);
            return await query.ToListAsync();
        }

        public virtual async Task<TDto> GetAsyncMapped<TDto>(params Expression<Func<T, bool>>[] expressions) where TDto : class
        {
            var entity = await GetAsync(expressions);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<IEnumerable<TDto>> FindAsyncMapped<TDto>(params Expression<Func<T, bool>>[] expressions) where TDto : class
        {
            var entities = await FindAsync(expressions);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
    }
}
