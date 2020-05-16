using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.Core.Helpers
{
    public static class JwtHelper
    {
        private static IConfiguration _configuration;

        //获取Startup构造函数中的Configuration对象
        public static void GetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Jwt加密
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string JwtEncrypt(TokenModelJwt tokenModel)
        {
            //获取配置文件中的信息
            var iss = _configuration["JwtTokenManagement:issuer"];
            var aud = _configuration["JwtTokenManagement:audience"];
            var secret = _configuration["JwtTokenManagement:secret"];

            //设置声明信息
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.UserId.ToString()),//Jwt唯一标识Id
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),//令牌签发时间
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,//不早于的时间声明
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddHours(24)).ToUnixTimeSeconds()}"),//令牌过期时间
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddHours(24).ToString(CultureInfo.CurrentCulture)),//令牌截至时间
                new Claim(JwtRegisteredClaimNames.Iss,iss),//发行人
                new Claim(JwtRegisteredClaimNames.Aud,aud),//订阅人
                new Claim(ClaimTypes.Role,tokenModel.Level)//权限——目前只支持单权限
            };

            //密钥处理，key+加密算法
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //封装成jwt对象
            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred
            );

            //生成返回jwt令牌
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Jwt解密
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt JwtDecrypt(string jwtStr)
        {
            if (string.IsNullOrEmpty(jwtStr) || string.IsNullOrWhiteSpace(jwtStr))
            {
                return new TokenModelJwt();
            }
            jwtStr = jwtStr.Substring(7);//截取前面的Bearer和空格
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            jwtToken.Payload.TryGetValue(ClaimTypes.Role, out object level);

            var model = new TokenModelJwt
            {
                UserId = Guid.Parse(jwtToken.Id),
                Level = level == null ? "" : level.ToString()
            };
            return model;
        }
    }

    /// <summary>
    /// 令牌包含的信息
    /// </summary>
    public class TokenModelJwt
    {
        public Guid UserId { get; set; }

        public string Level { get; set; }
    }
}
