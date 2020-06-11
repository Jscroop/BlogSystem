using Autofac;
using Autofac.Extras.DynamicProxy;
using BlogSystem.Common.Helpers;
using BlogSystem.Common.Helpers.SortHelper;
using BlogSystem.Core.AOP;
using BlogSystem.Core.Filters;
using BlogSystem.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlogSystem.Core.Test
{
    public class TestStartup
    {
        private readonly IConfiguration _configuration;

        public TestStartup(IConfiguration configuration)
        {
            _configuration = GetConfig(null);
            //传递Configuration对象
            JwtHelper.GetConfiguration(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //控制器服务注册
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;//开启不存在请求格式则返回406状态码的选项
                var jsonOutputFormatter = setup.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();//不为空则继续执行
                jsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.company.hateoas+json");
                setup.Filters.Add(typeof(ExceptionsFilter));//添加异常过滤器
            }).AddXmlDataContractSerializerFormatters()//开启输出输入支持XML格式
              .AddApplicationPart(Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.Core.dll")));

            //jwt授权服务注册
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //验证密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtTokenManagement:secret"])),

                    ValidateIssuer = true, //验证发行人
                    ValidIssuer = _configuration["JwtTokenManagement:issuer"],

                    ValidateAudience = true, //验证订阅人
                    ValidAudience = _configuration["JwtTokenManagement:audience"],

                    RequireExpirationTime = true, //验证过期时间
                    ValidateLifetime = true, //验证生命周期
                    ClockSkew = TimeSpan.Zero, //缓冲过期时间，即使配置了过期时间，也要考虑过期时间+缓冲时间
                };
            });

            //注册HttpContext存取器服务
            services.AddHttpContextAccessor();

            //自定义判断属性隐射关系
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<IPropertyCheckService, PropertyCheckService>();
        }

        //configureContainer访问AutoFac容器生成器
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //获取程序集并注册,采用每次请求都创建一个新的对象的模式
            var assemblyBll = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.BLL.dll"));
            var assemblyDal = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.DAL.dll"));

            builder.RegisterAssemblyTypes(assemblyDal).AsImplementedInterfaces().InstancePerDependency();

            //注册拦截器
            builder.RegisterType<LogAop>();
            //对目标类型启用动态代理，并注入自定义拦截器拦截BLL
            builder.RegisterAssemblyTypes(assemblyBll).AsImplementedInterfaces().InstancePerDependency()
           .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAop));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error!");
                    });
                });
            }

            app.UseRouting();

            //添加授权中间件
            app.UseAuthentication();

            //添加验证中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IConfiguration GetConfig(string environmentName)
        {
            var path = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(path)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
