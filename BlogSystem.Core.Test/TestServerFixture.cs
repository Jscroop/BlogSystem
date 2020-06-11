using Autofac.Extensions.DependencyInjection;
using BlogSystem.Core.Helpers;
using BlogSystem.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace BlogSystem.Core.Test
{
    public static class TestServerFixture
    {
        public static IHostBuilder GetTestHost()
        {
            return Host.CreateDefaultBuilder()
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())//使用autofac作为DI容器
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseTestServer()//建立TestServer——测试的关键
               .UseEnvironment("Development")
               .UseStartup<TestStartup>();
           });
        }

        //生成带token的httpclient
        public static HttpClient GetTestClientWithToken(this IHost host)
        {
            var client = host.GetTestClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GenerateJwtToken()}");//把token加到Header中
            return client;
        }

        //生成JwtToken
        public static string GenerateJwtToken()
        {
            TokenModelJwt tokenModel = new TokenModelJwt { UserId = userData.Id, Level = userData.Level.ToString() };
            var token = JwtHelper.JwtEncrypt(tokenModel);
            return token;
        }

        //测试用户的数据
        private static readonly User userData = new User
        {
            Account = "jordan",
            Id = new Guid("9CF2DAB5-B9DC-4910-98D8-CBB9D54E3D7B"),
            Level = Level.普通用户
        };

    }
}
