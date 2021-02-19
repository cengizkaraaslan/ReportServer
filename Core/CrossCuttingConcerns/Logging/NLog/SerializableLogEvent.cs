using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.NLog
{
   [Serializable]
   public class SerializableLogEvent
    {
        //Aşagıdaki kodlar log4net kullanılırsa açılır.biz Nlog kullandığımız için kapalı
        //LoggingEvent _loggingEvent;
        //public SerializableLogEvent(LoggingEvent loggingEvent)
        //{
        //    _loggingEvent = loggingEvent;
        //}
       // public object Message => __loggingEvent.MessageObject;
    }
}
