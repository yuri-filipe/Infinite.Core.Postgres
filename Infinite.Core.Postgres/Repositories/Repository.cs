namespace Infinite.Core.Postgres.Repositories
{
    using AutoMapper;
    using Infinite.Core.Postgres.Context;
    using Infinite.Core.Postgres.Mapping;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : CoreEntity
    {
        public Repository(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
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
