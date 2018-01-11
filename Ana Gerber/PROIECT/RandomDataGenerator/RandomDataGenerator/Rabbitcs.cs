using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator
{
    class Rabbitcs
    {
        string durl = "amqp://gnmbqahl:80sJnzzB7ZZLGMh5qZZES0UnyhQuYz1x@donkey.rmq.cloudamqp.com/gnmbqahl";
        string url = "amqp://gnmbqahl:80sJnzzB7ZZLGMh5qZZES0UnyhQuYz1x@donkey.rmq.cloudamqp.com/gnmbqahl";
        ConnectionFactory connFactory = new ConnectionFactory();

        public string Get()
        {
            connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var queueName = "am_primit";

                var data = channel.BasicGet(queueName, false);
                if (data == null) return null;

                var message = Encoding.UTF8.GetString(data.Body);

                channel.BasicAck(data.DeliveryTag, false);
                return message.ToString();
            }

        }
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
