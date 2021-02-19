using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;

namespace Core.CrossCuttingConcerns.Logging.NLog
{
   public class LoggerServiceBase
    {

        private Logger log;
        public LoggerServiceBase(string name)
        {
            log = LogManager.GetLogger(name);
        }
        public bool IsInfoEnabled => log.IsInfoEnabled;
        public bool IsDebugEnabled => log.IsDebugEnabled;
        public bool IsWarnEnabled => log.IsWarnEnabled;
        public bool IsFatalEnabled => log.IsFatalEnabled;
        public bool IsErrorEnabled => log.IsErrorEnabled;

        public void Info(object logMessage, object OutputData)
        {
            if (IsInfoEnabled)
                Task.Factory.StartNew(() => log.Info("Data {InputData}  created for {UserId} username {UserName} ", ConvertJson(logMessage),new HttpContext().GetUserId(), new HttpContext().GetUserName()));
        }
        public void Debug(object logMessage)
        {
            if (IsDebugEnabled)
                log.Debug(logMessage);
        }
        public void Warn(object logMessage)
        {
            if (IsWarnEnabled)
                log.Warn(logMessage);
        }
        public void Fatal(Exception ex, string logMessage, string Url)
        {
            if (IsFatalEnabled)
                Task.Factory.StartNew(() => log.Fatal(ex, "Data {Message} created for {UserId} , {Url}  username {UserName}", logMessage, new HttpContext().GetUserId(), Url, new HttpContext().GetUserName()));
        }
        public void Error(Exception ex, string message)
        {
            if (IsErrorEnabled)
                Task.Factory.StartNew(() => log.Error(ex, "Data {Message} created for {UserId}  username {UserName}", message, new HttpContext().GetUserId(), new HttpContext().GetUserName()));
        }
        private string ConvertJson(object value)
        {
            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
