using System;
using BlogSystem.Model;
using BlogSystem.Model.ViewModels;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Register(RegisterViewModel model);

        /// <summary>
        /// 登录成功返回userId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Login(LoginViewModel model);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(ChangePwdViewModel model, Guid userId);

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="profilePhoto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ChangeUserPhoto(string profilePhoto, Guid userId);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ChangeUserInfo(ChangeUserInfoViewModel model, Guid userId);

        /// <summary>
        /// 使用account获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<UserDetailsViewModel> GetUserInfoByAccount(string account);
    }
}
