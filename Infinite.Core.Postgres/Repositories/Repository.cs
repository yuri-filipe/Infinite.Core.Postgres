namespace Infinite.Core.Postgres.Repositories
{
    using AutoMapper;
    using Infinite.Core.Postgres.Context;
    using Infinite.Core.Postgres.Mapping;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    public class Repository<T> : IRepository<T> where T : CoreEntity<long>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public Repository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _mapper = mapper;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(List<Expression<Func<T, bool>>> expressions)
        {
            IQueryable<T> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TDto>> GetAsync<TDto>(List<Expression<Func<T, bool>>> expressions) where TDto : class
        {
            IQueryable<T> query = _dbSet;

            foreach (var expression in expressions)
            {
                query = query.Where(expression);
            }

            var entities = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> entities, int batchSize = 100)
        {
            int count = 0;
            foreach (var entity in entities)
            {
                await _dbSet.AddAsync(entity);
                count++;

                if (count % batchSize == 0)
                {
                    await _dbContext.SaveChangesAsync();
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).Property(e => e.Excluido).IsModified = false;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities, int batchSize = 100)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).Property(e => e.Excluido).IsModified = false;
                _dbSet.Update(entity);
                count++;

                if (count % batchSize == 0)
                {
                    await _dbContext.SaveChangesAsync();
                    _dbContext.ChangeTracker.Clear();
                }
            }

            if (count % batchSize != 0)
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.ChangeTracker.Clear();
            }
        }
        public async Task DeleteAsync(T entity)
        {
            entity.Excluido = true;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
