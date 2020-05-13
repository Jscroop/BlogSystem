using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSystem.Core.Helpers;
using BlogSystem.IBLL;
using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace BlogSystem.Core.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register", Name = nameof(Register))]
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
            if (userId != Guid.Empty)
            {
                //登录成功进行jwt加密
                var user = await _userService.GetOneByIdAsync(userId);
                TokenModelJwt tokenModel = new TokenModelJwt { UserId = user.Id, Level = user.Level.ToString() };
                var jwtStr = JwtHelper.JwtEncrypt(tokenModel);

                return Ok(jwtStr);

            }
            return Ok("账号或密码错误！");
        }

        [Authorize]
        [HttpPost("test")]
        public ActionResult Test()
        {
            return Ok();
        }

    }
}
