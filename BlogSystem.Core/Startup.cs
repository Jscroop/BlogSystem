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
            //����Configuration����
            JwtHelper.GetConfiguration(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //����������ע��
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;//���������������ʽ�򷵻�406״̬���ѡ��
            }).AddXmlDataContractSerializerFormatters();//�����������֧��XML��ʽ

            //SqlServer����ע��
            //services.AddDbContext<BlogSystemContext>(options =>
            //    options.UseSqlServer(_configuration.GetConnectionString("BlogSystemDbConnection")));

            //ע��Swagger����
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "BlogSystem API Doc-V1",
                    Description = "BlogSystem API�ӿ��ĵ�-V1��",
                    Contact = new OpenApiContact { Name = "BlogSystem", Email = "xxx@xx.com" },
                });
                options.OrderActionsBy(x => x.RelativePath);

                //����ע��,�ڶ���������controller��ע��
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BlogSystem.Core.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BlogSystem.Model.xml"));

                //��Ӧʱ��header�����jwt�����̨������������ȨС��
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //���÷���
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "��������ʱ���Bearer��һ���ո�",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            //jwt��Ȩ����ע��
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //��֤��Կ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtTokenManagement:secret"])),

                    ValidateIssuer = true, //��֤������
                    ValidIssuer = _configuration["JwtTokenManagement:issuer"],

                    ValidateAudience = true, //��֤������
                    ValidAudience = _configuration["JwtTokenManagement:audience"],

                    RequireExpirationTime = true, //��֤����ʱ��
                    ValidateLifetime = true, //��֤��������
                    ClockSkew = TimeSpan.Zero, //�������ʱ�䣬��ʹ�����˹���ʱ�䣬ҲҪ���ǹ���ʱ��+����ʱ��
                };
            });

            //ע��HttpContext��ȡ������
            services.AddHttpContextAccessor();
        }

        //configureContainer����AutoFac����������
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ȡ���򼯲�ע��,����ÿ�����󶼴���һ���µĶ����ģʽ
            var assemblyIBll = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.BLL.dll"));
            var assemblyIDal = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BlogSystem.DAL.dll"));
            builder.RegisterAssemblyTypes(assemblyIBll, assemblyIDal).AsImplementedInterfaces().InstancePerDependency();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Swagger�м������
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/V1/swagger.json", "ApiHelpDoc-V1");
                    x.RoutePrefix = "";//·����������Ϊ�գ���ʾֱ���ڸ��������ʸ��ļ�
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

            //�����Ȩ�м��
            app.UseAuthentication();

            //�����֤�м��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
