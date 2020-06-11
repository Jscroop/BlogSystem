using BlogSystem.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlogSystem.Core.Test.Controller_Test
{
    public class UserController_Should
    {
        const string _mediaType = "application/json";
        readonly Encoding _encoding = Encoding.UTF8;


        /// <summary>
        /// 用户注册
        /// </summary>
        [Fact]
        public async Task Register_Test()
        {
            // 1、Arrange
            var data = new RegisterViewModel { Account = "test", Password = "123456", RequirePassword = "123456" };

            StringContent content = new StringContent(JsonConvert.SerializeObject(data), _encoding, _mediaType);

            using var host = await TestServerFixture.GetTestHost().StartAsync();//启动TestServer

            // 2、Act
            var response = await host.GetTestClient().PostAsync($"http://localhost:5000/api/user/register", content);

            var result = await response.Content.ReadAsStringAsync();

            // 3、Assert
            Assert.DoesNotContain("用户已存在", result);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [Fact]
        public async Task Login_Test()
        {
            var data = new LoginViewModel { Account = "jordan", Password = "123456" };

            StringContent content = new StringContent(JsonConvert.SerializeObject(data), _encoding, _mediaType);

            var host = await TestServerFixture.GetTestHost().StartAsync();//启动TestServer

            var response = await host.GetTestClientWithToken().PostAsync($"http://localhost:5000/api/user/Login", content);

            var result = await response.Content.ReadAsStringAsync();

            Assert.DoesNotContain("账号或密码错误！", result);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [Fact]
        public async Task UserInfo_Test()
        {
            string id = "jordan";

            using var host = await TestServerFixture.GetTestHost().StartAsync();//启动TestServer

            var client = host.GetTestClient();

            var response = await client.GetAsync($"http://localhost:5000/api/user/{id}");

            var result = response.StatusCode;

            Assert.True(Equals(HttpStatusCode.OK, result)|| Equals(HttpStatusCode.NotFound, result));
        }
    }
}
