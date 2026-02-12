namespace Chet.CCLR.WebApi.Contracts
{
    /// <summary>
    /// 通用仓储接口，定义了实体的基本CRUD操作
    /// </summary>
    /// <typeparam name="T">实体类型，必须是引用类型</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象，如果不存在则返回null</returns>
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>如果实体存在则返回true，否则返回false</returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
