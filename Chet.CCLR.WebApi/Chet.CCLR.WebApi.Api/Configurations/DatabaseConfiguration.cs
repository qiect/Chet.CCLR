using Chet.CCLR.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Api.Configurations;

/// <summary>
/// 数据库配置类
/// </summary>
public static class DatabaseConfiguration
{
    /// <summary>
    /// 配置数据库
    /// </summary>
    /// <param name="services">IServiceCollection实例</param>
    /// <param name="configuration">IConfiguration实例</param>
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // 添加数据库上下文服务
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="app">WebApplication实例</param>
    public static void InitializeDatabase(this WebApplication app)
    {
        // 直接使用数据库上下文，不通过作用域，以减少服务解析
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // 确保数据库和表结构存在
        dbContext.Database.EnsureCreated();
    }
}
