using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace BlogSystem.Core.Filters
{
    public class ExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionsFilter> _logger;

        public ExceptionsFilter(ILogger<ExceptionsFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                //错误信息
                var msg = context.Exception.Message;
                //错误堆栈信息
                var stackTraceMsg = context.Exception.StackTrace;
                //返回信息
                context.Result = new InternalServerErrorObjectResult(new { msg, stackTraceMsg });
                //记录错误日志
                _logger.LogError(WriteLog(context.Exception));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                //记得释放，否则运行时无法打开
                Log.CloseAndFlush();
            }

        }

        //返回500错误
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        //自定义格式内容
        public string WriteLog(Exception ex)
        {
            return $"【异常信息】：{ex.Message} \r\n 【异常类型】：{ex.GetType().Name} \r\n【堆栈调用】：{ex.StackTrace}";
        }
    }
}
