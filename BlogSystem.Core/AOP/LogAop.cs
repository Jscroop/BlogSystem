using BlogSystem.Core.Helpers;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BlogSystem.Core.AOP
{
    public class LogAop : IInterceptor
    {
        private readonly IHttpContextAccessor _accessor;

        private static readonly string FileName = "AOPInterceptor-" + DateTime.Now.ToString("yyyyMMddHH") + ".log";

        //支持单个写线程和多个读线程的锁
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public LogAop(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public void Intercept(IInvocation invocation)
        {
            var userId = JwtHelper.JwtDecrypt(_accessor.HttpContext.Request.Headers["Authorization"]).UserId;

            //记录被拦截方法执行前的信息
            var logData = $"【执行用户】：{userId} \r\n" +
                          $"【执行时间】：{DateTime.Now:yyyy/MM/dd HH:mm:ss}  \r\n" +
                          $"【执行方法】: {invocation.Method.Name}  \r\n" +
                          $"【执行参数】：{string.Join(", ", invocation.Arguments.Select(x => (x ?? "").ToString()).ToArray())} \r\n";
            try
            {
                //调用下一个拦截器直到目标方法
                invocation.Proceed();

                //判断是否为异步方法
                if (IsAsyncMethod(invocation.Method))
                {
                    var type = invocation.Method.ReturnType;
                    var resultProperty = type.GetProperty("Result");
                    if (resultProperty == null) return;
                    var result = resultProperty.GetValue(invocation.ReturnValue);
                    logData += $"【执行完成】：{JsonConvert.SerializeObject(result)}";
                    Parallel.For(0, 1, e =>
                    {
                        WriteLog(new[] { logData });
                    });
                }
                else//同步方法
                {
                    logData += $"【执行完成】：{invocation.ReturnValue}";
                    Parallel.For(0, 1, e =>
                    {
                        WriteLog(new[] { logData });
                    });
                }

            }
            catch (Exception ex)
            {
                LogException(ex, logData);
            }
        }

        //判断是否为异步方法
        private bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(Task) ||
                   method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }

        //日志写入方法
        public static void WriteLog(string[] parameters, bool isHeader = true)
        {
            try
            {
                //进入写模式
                Lock.EnterWriteLock();

                //获取或创建文件夹
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //获取log文件路径
                var logFilePath = Path.Combine(path, FileName);

                //转换及拼接字符
                var logContent = string.Join("\r\n", parameters);
                if (isHeader)
                {
                    logContent = "---------------------------------------\r\n"
                                 + DateTime.Now + "\r\n" + logContent + "\r\n";
                }

                //写入文件
                File.AppendAllText(logFilePath, logContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                //退出写入模式，释放资源占用
                Lock.ExitWriteLock();
            }
        }

        //记录异常信息
        private void LogException(Exception ex, string logData)
        {
            if (ex == null) return;

            logData += $"【出现异常】：{ex.Message + ex.InnerException}\r\n";

            Parallel.For(0, 1, e =>
            {
                WriteLog(new[] { logData });
            });
        }
    }
}
