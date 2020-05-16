using BlogSystem.Core.Helpers;
using BlogSystem.IBLL;
using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlogSystem.Core.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Guid _userId;

        public UserController(IUserService userService, IHttpContextAccessor httpContext)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            var accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!await _userService.Register(model))
            {
                return Ok("用户已存在");
            }
            //创建成功返回到登录方法，并返回注册成功的account
            return CreatedAtRoute(nameof(Login), model.Account);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login", Name = nameof(Login))]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //判断账号密码是否正确
            var userId = await _userService.Login(model);
            if (userId == Guid.Empty) return Ok("账号或密码错误！");

            //登录成功进行jwt加密
            var user = await _userService.GetOneByIdAsync(userId);
            TokenModelJwt tokenModel = new TokenModelJwt { UserId = user.Id, Level = user.Level.ToString() };
            var jwtStr = JwtHelper.JwtEncrypt(tokenModel);
            return Ok(jwtStr);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("{account}")]
        public async Task<IActionResult> UserInfo(string account)
        {
            var list = await _userService.GetUserInfoByAccount(account);
            if (string.IsNullOrEmpty(list.Account))
            {
                return NotFound();
            }
            return Ok(list);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword(ChangePwdViewModel model)
        {
            if (!await _userService.ChangePassword(model, _userId))
            {
                return NotFound("用户密码错误！");
            }
            return NoContent();
        }

        /// <summary>
        /// 修改用户照片
        /// </summary>
        /// <param name="profilePhoto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("photo")]
        public async Task<IActionResult> ChangeUserPhoto([FromBody]string profilePhoto)
        {
            if (!await _userService.ChangeUserPhoto(profilePhoto, _userId))
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        ///  修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("info")]
        public async Task<IActionResult> ChangeUserInfo(ChangeUserInfoViewModel model)
        {
            if (!await _userService.ChangeUserInfo(model, _userId))
            {
                return Ok("用户名已存在");
            }
            return NoContent();
        }
    }
}
