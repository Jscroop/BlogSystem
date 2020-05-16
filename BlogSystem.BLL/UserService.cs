using BlogSystem.Common.Helpers;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using BlogSystem.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            BaseRepository = userRepository;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Register(RegisterViewModel model)
        {
            //判断账户是否存在
            if (await _userRepository.GetAll().AnyAsync(m => m.Account == model.Account))
            {
                return false;
            }
            var pwd = Md5Helper.Md5Encrypt(model.Password);
            await _userRepository.CreateAsync(new User
            {
                Account = model.Account,
                Password = pwd
            });
            return true;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Guid> Login(LoginViewModel model)
        {
            var pwd = Md5Helper.Md5Encrypt(model.Password);
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(m => m.Account == model.Account && m.Password == pwd);
            return user == null ? new Guid() : user.Id;
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(ChangePwdViewModel model, Guid userId)
        {
            var oldPwd = Md5Helper.Md5Encrypt(model.OldPassword);
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(m => m.Id == userId && m.Password == oldPwd);
            if (user == null)
            {
                return false;
            }
            var newPwd = Md5Helper.Md5Encrypt(model.NewPassword);
            user.Password = newPwd;
            await _userRepository.EditAsync(user);
            return true;
        }

        /// <summary>
        /// 修改用户照片
        /// </summary>
        /// <param name="profilePhoto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUserPhoto(string profilePhoto, Guid userId)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(m => m.Id == userId);
            if (user == null) return false;
            user.ProfilePhoto = profilePhoto;
            await _userRepository.EditAsync(user);
            return true;
        }

        /// <summary>
        ///  修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUserInfo(ChangeUserInfoViewModel model, Guid userId)
        {
            //确保用户名唯一
            if (await _userRepository.GetAll().AnyAsync(m => m.Account == model.Account))
            {
                return false;
            }
            var user = await _userRepository.GetOneByIdAsync(userId);
            user.Account = model.Account;
            user.Gender = model.Gender;
            user.BirthOfDate = model.BirthOfDate;
            await _userRepository.EditAsync(user);
            return true;
        }

        /// <summary>
        /// 通过账号名称获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<UserDetailsViewModel> GetUserInfoByAccount(string account)
        {
            if (await _userRepository.GetAll().AnyAsync(m => m.Account == account))
            {
                return await _userRepository.GetAll().Where(m => m.Account == account).Select(m =>
                    new UserDetailsViewModel()
                    {
                        Account = m.Account,
                        ProfilePhoto = m.ProfilePhoto,
                        Age = DateTime.Now.Year - m.BirthOfDate.Year,
                        Gender = m.Gender.ToString(),
                        Level = m.Level.ToString(),
                        FansNum = m.FansNum,
                        FocusNum = m.FocusNum
                    }).FirstAsync();
            }
            return new UserDetailsViewModel();
        }
    }
}
