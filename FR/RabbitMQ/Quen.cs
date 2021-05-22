using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FR.RabbitMQ
{
    public class Quen
    {
        public Quen(string AppName, string ReportName, string Extension, string Parameters, string connectionId)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "rabbitmq",
                Password = "rabbitmq",
                Port = 5672

            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Report",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                byte[] body = Encoding.Default.GetBytes(JsonConvert.SerializeObject(

                    new Models.RabbitMQ
                    {
                        AppName = AppName,
                        ReportName = ReportName,
                        Extension = Extension,
                        Parameters = Parameters,
                        connectionId = connectionId
                    }
                    ));

                channel.BasicPublish(exchange: "",
                                     routingKey: "Report",
                                     basicProperties: null,
                                     body: body);

            }
        }
    }
}
