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
            if (!await _userRepository.GetAll().AnyAsync(m => m.Account == model.Account))
            {
                var pwd = Md5Helper.Md5Encrypt(model.Password);
                await _userRepository.CreateAsync(new User()
                {
                    Account = model.Account,
                    Password = pwd
                });
                return true;
            }
            return false;
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
            return user != null ? user.Id : new Guid();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(ChangePwdViewModel model)
        {
            var oldPwd = Md5Helper.Md5Encrypt(model.OldPassword);
            var newPwd = Md5Helper.Md5Encrypt(model.NewPassword);
            if (await _userRepository.GetAll().AnyAsync(m => m.Id == model.UserId && m.Password == oldPwd))
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(m => m.Id == model.UserId && m.Password == oldPwd);
                user.Password = newPwd;
                await _userRepository.EditAsync(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改用户照片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ChangeUserPhoto(ChangeUserPhotoViewModel model)
        {
            var user = await _userRepository.GetAll().FirstAsync(m => m.Id == model.UserId);
            user.ProfilePhoto = model.ProfilePhoto;
            await _userRepository.EditAsync(user);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ChangeUserInfo(ChangeUserInfoViewModel model)
        {
            var user = await _userRepository.GetAll().FirstAsync(m => m.Id == model.UserId);
            user.Account = model.Account;
            user.Gender = model.Gender;
            user.BirthOfDate = model.BirthOfDate;
            await _userRepository.EditAsync(user);
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
                        UserId = m.Id,
                        Account = m.Account,
                        ProfilePhoto = m.ProfilePhoto,
                        Age = DateTime.Now.Year - m.BirthOfDate.Year,
                        Gender = m.Gender,
                        Level = m.Level,
                        FansNum = m.FansNum,
                        FocusNum = m.FocusNum
                    }).FirstAsync();
            }
            return new UserDetailsViewModel();
        }
    }
}
