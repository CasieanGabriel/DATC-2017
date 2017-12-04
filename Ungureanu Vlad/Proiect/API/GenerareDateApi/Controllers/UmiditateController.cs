using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GenerareDateApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Umiditate")]
    public class UmiditateController : Controller
    {
        // GET: api/Umiditate
        [HttpGet]
        public IEnumerable<string> GetUm()
        {
            List<string> listaSenzori = new List<string>();
            string message = "";
            string url = "";
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "queueUmiditate", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("queueUmiditate", false, consumer);
                    BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    var body = ea.Body;
                    message = Encoding.UTF8.GetString(body);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            }
            listaSenzori.Add(message);
            return listaSenzori.ToArray();
        }        
    }
}
