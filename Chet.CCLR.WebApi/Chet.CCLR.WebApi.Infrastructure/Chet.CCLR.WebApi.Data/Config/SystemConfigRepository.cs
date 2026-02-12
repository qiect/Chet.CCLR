using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Config;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data.Repositories;

/// <summary>
/// 系统配置仓储实现
/// </summary>
public class SystemConfigRepository : EfCoreRepository<SystemConfig>, ISystemConfigRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="context">数据库上下文</param>
    public SystemConfigRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// 根据配置键获取配置
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>配置信息</returns>
    public async Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.SystemConfigs
            .FirstOrDefaultAsync(c => c.ConfigKey == key, cancellationToken);
    }

    /// <summary>
    /// 获取公开配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公开配置列表</returns>
    public async Task<IEnumerable<SystemConfig>> GetPublicConfigsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SystemConfigs
            .Where(c => c.IsPublic)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 检查配置键是否存在
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public async Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.SystemConfigs.AnyAsync(c => c.ConfigKey == key, cancellationToken);
    }

    /// <summary>
    /// 更新配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="value">新值</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>操作结果</returns>
    public async Task<bool> UpdateValueAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var config = await _context.SystemConfigs
            .FirstOrDefaultAsync(c => c.ConfigKey == key, cancellationToken);

        if (config != null)
        {
            config.ConfigValue = value;
            config.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}