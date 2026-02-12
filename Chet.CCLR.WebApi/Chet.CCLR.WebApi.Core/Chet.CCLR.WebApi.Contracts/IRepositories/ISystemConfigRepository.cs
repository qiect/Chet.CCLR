using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Config;

namespace Chet.CCLR.WebApi.Contracts.IRepositories;

/// <summary>
/// 系统配置仓储接口
/// </summary>
public interface ISystemConfigRepository : IRepository<SystemConfig>
{
    /// <summary>
    /// 根据配置键获取配置
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>配置信息</returns>
    Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取公开配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公开配置列表</returns>
    Task<IEnumerable<SystemConfig>> GetPublicConfigsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查配置键是否存在
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="value">新值</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    Task<bool> UpdateValueAsync(string key, string value, CancellationToken cancellationToken = default);
}