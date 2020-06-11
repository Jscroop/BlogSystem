using System.IO;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace BlogSystem.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //设定最小记录级别
                .MinimumLevel.Debug()
                //遇到Microsoft命名空间，重写记录级别
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                //输出到控制台
                .WriteTo.Console()
                //输出到本地，配置每小时生成一个log文件
                .WriteTo.File(Path.Combine("Logs", "log-.log"), rollingInterval: RollingInterval.Hour)
                //创建log对象
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //将默认的ServiceProviderFactory替换为AutoFac服务提供工厂
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog(dispose: true)//添加使用Serilog
                        .UseStartup<Startup>();
                });
    }
}
