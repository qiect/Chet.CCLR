using AutoMapper;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain.Config;
using Chet.CCLR.WebApi.DTOs.Config;

namespace Chet.CCLR.WebApi.Services.Config;

/// <summary>
/// 系统配置服务实现
/// </summary>
public class SystemConfigService : ISystemConfigService
{
    private readonly ISystemConfigRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">配置仓储</param>
    /// <param name="mapper">对象映射器</param>
    public SystemConfigService(ISystemConfigRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<string?> GetConfigValueAsync(string key, CancellationToken cancellationToken = default)
    {
        var config = await _repository.GetByKeyAsync(key, cancellationToken);
        return config?.ConfigValue;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ConfigResponseDto>> GetPublicConfigsAsync(CancellationToken cancellationToken = default)
    {
        var configs = await _repository.GetPublicConfigsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ConfigResponseDto>>(configs);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ConfigResponseDto>> GetAllConfigsAsync(CancellationToken cancellationToken = default)
    {
        var configs = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ConfigResponseDto>>(configs);
    }

    /// <inheritdoc />
    public async Task<bool> SetConfigValueAsync(SetConfigRequestDto request, CancellationToken cancellationToken = default)
    {
        var config = await _repository.GetByKeyAsync(request.Key, cancellationToken);
        if (config != null)
        {
            // 更新现有配置
            config.ConfigValue = request.Value;
            config.Description = request.Description;
            config.IsPublic = request.IsPublic;
            config.UpdatedAt = DateTime.Now;
            await _repository.UpdateAsync(config, cancellationToken);
        }
        else
        {
            // 创建新配置
            config = new SystemConfig
            {
                ConfigKey = request.Key,
                ConfigValue = request.Value,
                Description = request.Description,
                IsPublic = request.IsPublic,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _repository.AddAsync(config, cancellationToken);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> BatchUpdateConfigsAsync(IEnumerable<SetConfigRequestDto> requests, CancellationToken cancellationToken = default)
    {
        foreach (var request in requests)
        {
            await SetConfigValueAsync(request, cancellationToken);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsConfigKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _repository.ExistsByKeyAsync(key, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteConfigAsync(string key, CancellationToken cancellationToken = default)
    {
        var config = await _repository.GetByKeyAsync(key, cancellationToken);
        if (config == null)
        {
            return false;
        }

        await _repository.DeleteAsync(config, cancellationToken);
        return true;
    }
}