using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Castle.Core.Configuration;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using NLog;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopwatch;
        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }
        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval) //Elapse gecen süre
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}"); //DeclaringType class adı
            }
            Task.Run(() =>
            {


                var a = LogManager.Configuration.Variables["fromUserName"];
                var b = LogManager.Configuration.Variables["smtpPassword"];
                var c = LogManager.Configuration.Variables["smtpServer"];
                var d = LogManager.Configuration.Variables["smtpUseSSL"];
         

                //var a = ServiceTool.ServiceProvider.GetService<IConfiguration>();
                //smtp = new Core.Mail.SmtpEmail();
                //MessagePackage messagePackage = new MessagePackage
                //{
                //    Attachments = null,
                //};
                //List<UserAddress> userAddress = new List<UserAddress>();
                //userAddress.Add(new UserAddress { Address = Email });
                //messagePackage.To = userAddress;
                //messagePackage.MailTemplateType = MailTemplateType.ForgotPassword;
                //messagePackage.Body = Message;
                //messagePackage.Subject = Message;
                //smtp.Send(messagePackage);
            });
            _stopwatch.Reset();
        }
    }
}
