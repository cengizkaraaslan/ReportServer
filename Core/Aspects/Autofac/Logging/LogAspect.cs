using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.NLog;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using System.Web;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private LoggerServiceBase _loggerServiceBase;

        public LogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new System.Exception(AspectMessages.WrongLoggerType);
            }
            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
        }
        private LogDetail GetLogDetail(IInvocation invocation)
        {

            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name
                });
            }

            var logDetail = new LogDetail
            {
                MethodName = invocation.Method.Name,
                LogParameters = logParameters,
                UserId = new HttpContext().GetUserId(),
                //OutputData = OutPutData
            };

            return logDetail;
        }
        protected override void OnAfter(IInvocation invocation)
        {
            Task.Run(() =>
            {
                var method = invocation.MethodInvocationTarget;
                var isAsync = method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
                if (isAsync && typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    Task t = InterceptAsync((dynamic)invocation.ReturnValue) as Task;
                    if (t.IsCompleted)
                    {
                        _loggerServiceBase.Info(GetLogDetail(invocation), (t as dynamic).Result);
                    }
                }
                base.OnAfter(invocation);
            });
        }
        private async Task<T> InterceptAsync<T>(Task<T> task)
        {
            T result = await task.ConfigureAwait(false);

            return result;
        }
    }
}
