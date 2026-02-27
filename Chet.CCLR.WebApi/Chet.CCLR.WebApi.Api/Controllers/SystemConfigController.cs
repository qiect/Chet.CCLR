using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.DTOs.Config;
using Chet.CCLR.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chet.CCLR.WebApi.Api.Controllers;

/// <summary>
/// 系统配置控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("提供系统配置管理相关的API接口，包括获取、设置和删除配置")]
public class SystemConfigController : ControllerBase
{
    /// <summary>
    /// 配置服务，用于处理配置相关的业务逻辑
    /// </summary>
    private readonly ISystemConfigService _configService;

    /// <summary>
    /// 日志记录器，用于记录控制器操作日志
    /// </summary>
    private readonly ILogger<SystemConfigController> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configService">配置服务</param>
    public SystemConfigController(ISystemConfigService configService, ILogger<SystemConfigController> logger)
    {
        _configService = configService;
        _logger = logger;
    }

    /// <summary>
    /// 获取配置值
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>配置值</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/SystemConfig/Key
    ///     {
    ///         "success": true,
    ///         "data": "配置值",
    ///         "message": "Config value retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回配置值</response>
    /// <response code="404">配置不存在</response>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConfigValue(string key)
    {
        _logger.LogInformation("Getting config value for key: {Key}", key);
        var value = await _configService.GetConfigValueAsync(key);
        if (value == null)
        {
            return Ok(ApiResponse.Error("Config value not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.Ok(value, "Config value retrieved successfully"));
    }

    /// <summary>
    /// 获取公共配置
    /// </summary>
    /// <returns>公共配置列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/SystemConfig/public
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "key": "SiteName",
    ///                 "value": "配置值",
    ///                 "isPublic": true
    ///             }
    ///         ],
    ///         "message": "Public configs retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回公共配置列表</response>
    [HttpGet("public")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublicConfigs()
    {
        _logger.LogInformation("Getting public configs");
        var configs = await _configService.GetPublicConfigsAsync();
        return Ok(ApiResponse.Ok(configs, "Public configs retrieved successfully"));
    }

    /// <summary>
    /// 获取所有配置
    /// </summary>
    /// <returns>所有配置列表</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/SystemConfig
    ///     {
    ///         "success": true,
    ///         "data": [
    ///             {
    ///                 "key": "Key1",
    ///                 "value": "配置值1",
    ///                 "isPublic": true
    ///             },
    ///             {
    ///                 "key": "Key2",
    ///                 "value": "配置值2",
    ///                 "isPublic": false
    ///             }
    ///         ],
    ///         "message": "All configs retrieved successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">获取成功，返回所有配置列表</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllConfigs()
    {
        _logger.LogInformation("Getting all configs");
        var configs = await _configService.GetAllConfigsAsync();
        return Ok(ApiResponse.Ok(configs, "All configs retrieved successfully"));
    }

    /// <summary>
    /// 设置配置值
    /// </summary>
    /// <param name="request">设置请求</param>
    /// <returns>设置结果</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/SystemConfig
    ///     {
    ///         "key": "NewKey",
    ///         "value": "NewValue",
    ///         "isPublic": true
    ///     }
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Config value set successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">设置成功</response>
    /// <response code="400">设置失败，输入无效</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetConfigValue([FromBody] SetConfigRequestDto request)
    {
        _logger.LogInformation("Setting config value for key: {Key}", request.Key);
        var result = await _configService.SetConfigValueAsync(request);
        if (result)
        {
            return Ok(ApiResponse.Ok(null, "Config value set successfully"));
        }
        return Ok(ApiResponse.Error("Failed to set config value", StatusCodes.Status400BadRequest));
    }

    /// <summary>
    /// 批量更新配置
    /// </summary>
    /// <param name="requests">批量请求</param>
    /// <returns>更新结果</returns>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     POST /api/SystemConfig/batch-update
    ///     [
    ///         {
    ///             "key": "Key1",
    ///             "value": "Value1",
    ///             "isPublic": true
    ///         },
    ///         {
    ///             "key": "Key2",
    ///             "value": "Value2",
    ///             "isPublic": false
    ///         }
    ///     ]
    /// 
    /// 示例响应：
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Configs updated successfully",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">更新成功</response>
    /// <response code="400">更新失败，输入无效</response>
    [HttpPost("batch-update")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchUpdateConfigs([FromBody] IEnumerable<SetConfigRequestDto> requests)
    {
        _logger.LogInformation("Batch updating {Count} configs", requests.Count());
        var result = await _configService.BatchUpdateConfigsAsync(requests);
        if (result)
        {
            return Ok(ApiResponse.Ok(null, "Configs updated successfully"));
        }
        return Ok(ApiResponse.Error("Failed to update configs", StatusCodes.Status400BadRequest));
    }

    /// <summary>
    /// 检查配置键是否存在
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>是否存在</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     GET /api/SystemConfig/exists/Key
    ///     {
    ///         "success": true,
    ///         "data": true,
    ///         "message": "Config key check completed",
    ///         "statusCode": 200
    ///     }
    /// </remarks>
    /// <response code="200">检查完成，返回是否存在</response>
    [HttpGet("exists/{key}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExistsConfigKey(string key)
    {
        _logger.LogInformation("Checking if config key exists: {Key}", key);
        var exists = await _configService.ExistsConfigKeyAsync(key);
        return Ok(ApiResponse.Ok(exists, "Config key check completed"));
    }

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="key">配置键</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 示例响应：
    /// 
    ///     DELETE /api/SystemConfig/Key
    ///     {
    ///         "success": true,
    ///         "data": null,
    ///         "message": "Config deleted successfully",
    ///         "statusCode": 204
    ///     }
    /// </remarks>
    /// <response code="204">删除成功</response>
    /// <response code="404">配置不存在</response>
    [HttpDelete("{key}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteConfig(string key)
    {
        _logger.LogInformation("Deleting config with key: {Key}", key);
        var result = await _configService.DeleteConfigAsync(key);
        if (!result)
        {
            return Ok(ApiResponse.Error("Config not found", StatusCodes.Status404NotFound));
        }
        return Ok(ApiResponse.NoContent("Config deleted successfully"));
    }
}
