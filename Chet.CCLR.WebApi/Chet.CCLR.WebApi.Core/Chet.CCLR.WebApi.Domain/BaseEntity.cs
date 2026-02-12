namespace Chet.CCLR.WebApi.Domain
{
    /// <summary>
    /// 实体基类，包含所有实体共有的字段
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 实体唯一标识符
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// 实体创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 实体更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
