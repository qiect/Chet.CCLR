# EF Core 迁移命令指南

## 概述
本文档介绍了在Chet.CCLR项目中使用EF Core迁移的相关命令，包括在命令行和Visual Studio程序包管理器控制台中的使用方式。

## 命令行方式

### 1. 创建迁移文件
```bash
cd "e:\Project\Chet.CCLR\Chet.CCLR.WebApi\Chet.CCLR.WebApi.Infrastructure\Chet.CCLR.WebApi.Data"
dotnet ef migrations add [MigrationName] --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"
```

**示例：**
```bash
dotnet ef migrations add InitialCreate --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"
```

### 2. 更新数据库（应用迁移）
```bash
cd "e:\Project\Chet.CCLR\Chet.CCLR.WebApi\Chet.CCLR.WebApi.Infrastructure\Chet.CCLR.WebApi.Data"
dotnet ef database update --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"
```

### 3. 其他常用命令
```bash
# 查看迁移历史
dotnet ef migrations list --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"

# 删除最后一次迁移
dotnet ef migrations remove --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"

# 生成SQL脚本
dotnet ef migrations script --project "Chet.CCLR.WebApi.Data.csproj" --startup-project "..\..\Chet.CCLR.WebApi.Api\Chet.CCLR.WebApi.Api.csproj"
```

## Visual Studio 程序包管理器控制台方式

### 1. 创建迁移
```powershell
Add-Migration InitialCreate -Project Chet.CCLR.WebApi.Data -StartupProject Chet.CCLR.WebApi.Api
```

### 2. 更新数据库
```powershell
Update-Database -Project Chet.CCLR.WebApi.Data -StartupProject Chet.CCLR.WebApi.Api
```

### 3. 其他命令
```powershell
# 查看迁移状态
Get-Migration -Project Chet.CCLR.WebApi.Data -StartupProject Chet.CCLR.WebApi.Api

# 删除最近一次迁移
Remove-Migration -Project Chet.CCLR.WebApi.Data -StartupProject Chet.CCLR.WebApi.Api

# 应用到特定迁移
Update-Database -Migration "MigrationName" -Project Chet.CCLR.WebApi.Data -StartupProject Chet.CCLR.WebApi.Api
```

## 注意事项

1. **项目依赖**：确保启动项目（API项目）包含 `Microsoft.EntityFrameworkCore.Design` 包引用
2. **类型一致性**：实体间的关系属性类型需保持一致（如Guid与Guid对应，不能混用Guid与string）
3. **路径指定**：命令行方式需在包含AppDbContext的Data项目目录下执行
4. **项目名称**：程序包管理器控制台使用项目名称而非完整路径
5. **权限问题**：如遇文件被占用问题，可先执行 `dotnet clean` 清理后再操作

## 迁移最佳实践

1. 在进行重大数据库变更前备份数据
2. 为每个功能模块创建独立的迁移
3. 定期审查生成的迁移代码，确保其符合预期
4. 在开发环境中测试迁移后再部署到生产环境