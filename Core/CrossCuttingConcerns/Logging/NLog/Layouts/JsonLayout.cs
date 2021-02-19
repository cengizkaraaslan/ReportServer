using Newtonsoft.Json;
using NLog;
using NLog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.NLog.Layouts
{
   public class JsonLayout : IJsonConverter
    {
        readonly JsonSerializerSettings _settings;

        public JsonLayout()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public bool SerializeObject(object value, StringBuilder builder)
        {
            try
            {
                var jsonSerializer = JsonSerializer.CreateDefault(_settings);
                var sw = new System.IO.StringWriter(builder, System.Globalization.CultureInfo.InvariantCulture);
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = jsonSerializer.Formatting;
                    jsonSerializer.Serialize(jsonWriter, value, null);
                }
            }
            catch (Exception e)
            {
                InternalLogger.Error(e, "Error when custom JSON serialization");
                return false;
            }
            return true;
        }
    }
}
