using System.ComponentModel.DataAnnotations;

namespace Chet.CCLR.WebApi.Domain.Log
{
    /// <summary>
    /// 操作日志实体类，用于记录系统的操作日志
    /// </summary>
    public class OperationLog : BaseEntity
    {
        /// <summary>
        /// 操作类型（如：Create, Update, Delete, Query等）
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// 目标实体ID
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 关联的用户ID
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 关联的用户
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 额外的数据(JSON格式)
        /// </summary>
        public string? ExtraData { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User-Agent
        /// </summary>
        public string? UserAgent { get; set; }
    }
}