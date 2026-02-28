using Chet.CCLR.WebApi.DTOs;
using Chet.CCLR.WebApi.DTOs.User;

namespace Chet.CCLR.WebApi.Contracts
{
    /// <summary>
    /// 用户服务接口，定义了用户相关的业务逻辑操作
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户DTO</returns>
        Task<UserDto> GetUserByIdAsync(Guid id);

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns>用户DTO列表</returns>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="userCreateDto">用户创建DTO</param>
        /// <returns>创建的用户DTO</returns>
        Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="userUpdateDto">用户更新DTO</param>
        Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        Task DeleteUserAsync(Guid id);

        /// <summary>
        /// 根据微信OpenID获取用户信息
        /// </summary>
        /// <param name="wxOpenid">微信OpenID</param>
        /// <returns>用户DTO，如果不存在则返回null</returns>
        Task<UserDto?> GetUserByWxOpenidAsync(string wxOpenid);
    }
}
