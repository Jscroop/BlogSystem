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
                //�趨��С��¼����
                .MinimumLevel.Debug()
                //����Microsoft�����ռ䣬��д��¼����
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                //���������̨
                .WriteTo.Console()
                //��������أ�����ÿСʱ����һ��log�ļ�
                .WriteTo.File(Path.Combine("Logs", "log-.log"), rollingInterval: RollingInterval.Hour)
                //����log����
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //��Ĭ�ϵ�ServiceProviderFactory�滻ΪAutoFac�����ṩ����
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog(dispose: true)//���ʹ��Serilog
                        .UseStartup<Startup>();
                });
    }
}
