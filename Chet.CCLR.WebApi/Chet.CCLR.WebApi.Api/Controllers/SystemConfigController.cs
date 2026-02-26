using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Config;
using Microsoft.AspNetCore.Mvc;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 系统配置控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SystemConfigController : ControllerBase
{
    private readonly ISystemConfigService _configService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configService">配置服务</param>
    public SystemConfigController(ISystemConfigService configService)
    {
        _configService = configService;
    }

    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>配置值</returns>
    [HttpGet("{key}")]
    public async Task<ActionResult<string>> GetConfigValue(string key)
    {
        var value = await _configService.GetConfigValueAsync(key);
        if (value == null)
        {
            return NotFound();
        }
        return Ok(value);
    }

    /// <summary>
    /// 获取公共配置
    /// </summary>
    /// <returns>公共配置列表</returns>
    [HttpGet("public")]
    public async Task<ActionResult<IEnumerable<ConfigResponseDto>>> GetPublicConfigs()
    {
        var configs = await _configService.GetPublicConfigsAsync();
        return Ok(configs);
    }

    /// <summary>
    /// 获取所有配置
    /// </summary>
    /// <returns>所有配置列表</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigResponseDto>>> GetAllConfigs()
    {
        var configs = await _configService.GetAllConfigsAsync();
        return Ok(configs);
    }

    /// <summary>
    /// 设置配置值
    /// </summary>
    /// <param name="request">设置请求</param>
    /// <returns>设置结果</returns>
    [HttpPost]
    public async Task<IActionResult> SetConfigValue([FromBody] SetConfigRequestDto request)
    {
        var result = await _configService.SetConfigValueAsync(request);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }

    /// <summary>
    /// 批量更新配置
    /// </summary>
    /// <param name="requests">批量请求</param>
    /// <returns>更新结果</returns>
    [HttpPost("batch-update")]
    public async Task<IActionResult> BatchUpdateConfigs([FromBody] IEnumerable<SetConfigRequestDto> requests)
    {
        var result = await _configService.BatchUpdateConfigsAsync(requests);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }

    /// <summary>
    /// 检查配置键是否存在
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>是否存在</returns>
    [HttpGet("exists/{key}")]
    public async Task<ActionResult<bool>> ExistsConfigKey(string key)
    {
        var exists = await _configService.ExistsConfigKeyAsync(key);
        return Ok(exists);
    }

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteConfig(string key)
    {
        var result = await _configService.DeleteConfigAsync(key);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}