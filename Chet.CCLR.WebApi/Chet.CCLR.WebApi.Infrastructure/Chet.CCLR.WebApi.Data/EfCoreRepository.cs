using Chet.CCLR.WebApi.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data
{
    /// <summary>
    /// EF Core通用仓储实现类，实现了IRepository<T>接口
    /// </summary>
    /// <typeparam name="T">实体类型，必须是引用类型</typeparam>
    public class EfCoreRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// 数据库上下文，用于与数据库交互
        /// </summary>
        protected readonly AppDbContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public EfCoreRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id) != null;
        }
    }
}
