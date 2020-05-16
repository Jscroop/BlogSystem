using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Autofac;
using BlogSystem.Core.Helpers;
using BlogSystem.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BlogSystem.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            //传递Configuration对象
            JwtHelper.GetConfiguration(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //控制器服务注册
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;//开启不存在请求格式则返回406状态码的选项
            }).AddXmlDataContractSerializerFormatters();//开启输出输入支持XML格式

            //SqlServer服务注册
            //services.AddDbContext<BlogSystemContext>(options =>
            //    options.UseSqlServer(_configuration.GetConnectionString("BlogSystemDbConnection")));

            //注册Swagger服务
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "BlogSystem API Doc-V1",
                    Description = "BlogSystem API接口文档-V1版",
                    Contact = new OpenApiContact { Name = "BlogSystem", Email = "xxx@xx.com" },
                });
                options.OrderActionsBy(x => x.RelativePath);

                //配置注释,第二个参数是controller的注释
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BlogSystem.Core.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BlogSystem.Model.xml"));

                //响应时在header中添加jwt传入后台方法；开启授权小锁
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //配置方法
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "请在输入时添加Bearer和一个空格",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

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
        }

        //configureContainer访问AutoFac容器生成器
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //获取程序集并注册,采用每次请求都创建一个新的对象的模式
            var assemblyIBll = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.BLL.dll"));
            var assemblyIDal = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.DAL.dll"));
            builder.RegisterAssemblyTypes(assemblyIBll, assemblyIDal).AsImplementedInterfaces().InstancePerDependency();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Swagger中间件配置
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/V1/swagger.json", "ApiHelpDoc-V1");
                    x.RoutePrefix = "";//路径配置设置为空，表示直接在根域名访问该文件
                });
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
    }
}
