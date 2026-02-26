using Chet.CCLR.WebApi.DTOs.Config;

namespace Chet.CCLR.WebApi.Contracts.IServices;

/// <summary>
/// 系统配置服务接口
/// </summary>
public interface ISystemConfigService
{
    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>配置值</returns>
    Task<string?> GetConfigValueAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取公共配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公共配置列表</returns>
    Task<IEnumerable<ConfigResponseDto>> GetPublicConfigsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取所有配置
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>所有配置列表</returns>
    Task<IEnumerable<ConfigResponseDto>> GetAllConfigsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 设置配置值
    /// </summary>
    /// <param name="request">设置请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否设置成功</returns>
    Task<bool> SetConfigValueAsync(SetConfigRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量更新配置
    /// </summary>
    /// <param name="requests">批量请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否批量更新成功</returns>
    Task<bool> BatchUpdateConfigsAsync(IEnumerable<SetConfigRequestDto> requests, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查配置键是否存在
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsConfigKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="key">配置键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeleteConfigAsync(string key, CancellationToken cancellationToken = default);
}