namespace Infinite.Core.Postgres.Repositories
{
    using AutoMapper;
    using Infinite.Core.Postgres.Context;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
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

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(List<Expression<Func<T, bool>>> expressions)
        {
            IQueryable<T> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TDto>> GetAsync<TDto>(List<Expression<Func<T, bool>>> expressions) where TDto : class
        {
            IQueryable<T> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Where(expression);
            }

            var entities = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
    }
}