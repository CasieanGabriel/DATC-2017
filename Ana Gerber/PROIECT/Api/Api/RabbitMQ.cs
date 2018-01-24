using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class RabbitMQ
    {
        string url = "amqp://gnmbqahl:80sJnzzB7ZZLGMh5qZZES0UnyhQuYz1x@donkey.rmq.cloudamqp.com/gnmbqahl";

        ConnectionFactory connFactory = new ConnectionFactory();

        public void Publish(string mesaj)
        {
            connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var data = Encoding.UTF8.GetBytes(mesaj);

                var exchangeName = "";
                var routingKey = "ti-am_trimis";
                channel.BasicPublish(exchangeName, routingKey, null, data);
            }

        }
    }
}
