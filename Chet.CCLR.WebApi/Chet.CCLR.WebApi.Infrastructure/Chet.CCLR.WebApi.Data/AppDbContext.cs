using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.Domain.Config;
using Chet.CCLR.WebApi.Domain.Log;
using Microsoft.EntityFrameworkCore;

namespace Chet.CCLR.WebApi.Data
{
    /// <summary>
    /// EF Core 数据库上下文类，用于管理实体和数据库交互
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">数据库上下文配置选项</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region User Management
        /// <summary>
        /// 表示数据库中的 Users 表
        /// </summary>
        public DbSet<User> Users { get; set; }
        #endregion

        #region Classic Content Management
        /// <summary>
        /// 表示数据库中的 ClassicBooks 表
        /// </summary>
        public DbSet<ClassicBook> ClassicBooks { get; set; }

        /// <summary>
        /// 表示数据库中的 ClassicChapters 表
        /// </summary>
        public DbSet<ClassicChapter> ClassicChapters { get; set; }

        /// <summary>
        /// 表示数据库中的 ClassicSentences 表
        /// </summary>
        public DbSet<ClassicSentence> ClassicSentences { get; set; }
        #endregion

        #region Listen Management
        /// <summary>
        /// 表示数据库中的 UserListenProgress 表
        /// </summary>
        public DbSet<UserListenProgress> UserListenProgress { get; set; }

        /// <summary>
        /// 表示数据库中的 UserListenRecord 表
        /// </summary>
        public DbSet<UserListenRecord> UserListenRecords { get; set; }

        /// <summary>
        /// 表示数据库中的 UserFavoriteSentences 表
        /// </summary>
        public DbSet<UserFavoriteSentence> UserFavoriteSentences { get; set; }
        #endregion

        #region Configuration and Logging
        /// <summary>
        /// 表示数据库中的 SystemConfigs 表
        /// </summary>
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        /// <summary>
        /// 表示数据库中的 OperationLogs 表
        /// </summary>
        public DbSet<OperationLog> OperationLogs { get; set; }
        #endregion

        /// <summary>
        /// 配置实体映射和关系
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置用户实体
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // 设置主键
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255); // 配置 Email 属性
                entity.HasIndex(e => e.Email).IsUnique(); // 为 Email 添加唯一索引
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100); // 配置 Name 属性
                entity.Property(e => e.PasswordHash).IsRequired(); // 配置 PasswordHash 属性
                entity.Property(e => e.CreatedAt).IsRequired(); // 配置 CreatedAt 属性
                entity.Property(e => e.UpdatedAt).IsRequired(); // 配置 UpdatedAt 属性
            });

            #region Classic Content Configuration
            // 配置经典书籍实体
            modelBuilder.Entity<ClassicBook>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Subtitle).HasMaxLength(200);
                entity.Property(e => e.Author).HasMaxLength(100);
                entity.Property(e => e.Dynasty).HasMaxLength(50);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.CoverImage).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与章节的关系
                entity.HasMany(e => e.Chapters)
                    .WithOne(e => e.Book)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 配置经典章节实体
            modelBuilder.Entity<ClassicChapter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与书籍的关系
                entity.HasOne(e => e.Book)
                    .WithMany(e => e.Chapters)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与句子的关系
                entity.HasMany(e => e.Sentences)
                    .WithOne(e => e.Chapter)
                    .HasForeignKey(e => e.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 配置经典句子实体
            modelBuilder.Entity<ClassicSentence>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(int.MaxValue);
                entity.Property(e => e.Pinyin).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Note).HasMaxLength(int.MaxValue);
                entity.Property(e => e.Translation).HasMaxLength(int.MaxValue);
                entity.Property(e => e.AudioUrl).IsRequired().HasMaxLength(int.MaxValue);
                entity.Property(e => e.AudioFormat).HasMaxLength(10);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与章节的关系
                entity.HasOne(e => e.Chapter)
                    .WithMany(e => e.Sentences)
                    .HasForeignKey(e => e.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与收藏的关系
                entity.HasMany(e => e.FavoriteRecords)
                    .WithOne(e => e.Sentence)
                    .HasForeignKey(e => e.SentenceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Listen Management Configuration
            // 配置用户听读进度实体
            modelBuilder.Entity<UserListenProgress>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.BookId).IsRequired();
                entity.Property(e => e.ChapterId).IsRequired();
                entity.Property(e => e.SentenceId).IsRequired();
                entity.Property(e => e.PlaySpeed).HasPrecision(2, 1);
                entity.Property(e => e.LastPositionPercent).HasPrecision(5, 2);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与用户的关系
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与书籍的关系
                entity.HasOne(e => e.Book)
                    .WithMany()
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与章节的关系
                entity.HasOne(e => e.Chapter)
                    .WithMany()
                    .HasForeignKey(e => e.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与句子的关系
                entity.HasOne(e => e.Sentence)
                    .WithMany()
                    .HasForeignKey(e => e.SentenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置唯一约束：用户和书籍的组合
                entity.HasIndex(e => new { e.UserId, e.BookId }).IsUnique();
            });

            // 配置用户听读记录实体
            modelBuilder.Entity<UserListenRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.BookId).IsRequired();
                entity.Property(e => e.SentenceIds).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与用户的关系
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与书籍的关系
                entity.HasOne(e => e.Book)
                    .WithMany()
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与章节的关系
                entity.HasOne(e => e.Chapter)
                    .WithMany()
                    .HasForeignKey(e => e.ChapterId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                // 配置唯一约束：用户和日期的组合
                entity.HasIndex(e => new { e.UserId, e.ListenDate }).IsUnique();
            });

            // 配置用户收藏句子实体
            modelBuilder.Entity<UserFavoriteSentence>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.SentenceId).IsRequired();
                entity.Property(e => e.Note).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置与用户的关系
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置与句子的关系
                entity.HasOne(e => e.Sentence)
                    .WithMany(e => e.FavoriteRecords)
                    .HasForeignKey(e => e.SentenceId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // 配置唯一约束：用户和句子的组合
                entity.HasIndex(e => new { e.UserId, e.SentenceId }).IsUnique();
            });
            #endregion

            #region Configuration and Logging Configuration
            // 配置系统配置实体
            modelBuilder.Entity<SystemConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConfigKey).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ConfigValue).IsRequired().HasMaxLength(int.MaxValue);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                // 配置唯一索引：配置键的唯一性
                entity.HasIndex(e => e.ConfigKey).IsUnique();
            });

            // 配置操作日志实体
            modelBuilder.Entity<OperationLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TargetType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.TargetId).IsRequired();
                entity.Property(e => e.ExtraData).HasMaxLength(int.MaxValue);
                entity.Property(e => e.IpAddress).HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CreatedAt).IsRequired();
                
                // 配置与用户的关系
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                // 配置索引
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Operation);
                entity.HasIndex(e => new { e.TargetType, e.TargetId });
                entity.HasIndex(e => e.CreatedAt);
            });
            #endregion
        }

        /// <summary>
        /// 重写基类方法，用于自动设置实体的创建和更新时间
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>影响的行数</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 自动设置创建和更新时间
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entities)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                entity.UpdatedAt = DateTime.Now; // 设置更新时间为当前 北京 时间

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.Now; // 新建实体时，设置创建时间为当前 北京 时间
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
