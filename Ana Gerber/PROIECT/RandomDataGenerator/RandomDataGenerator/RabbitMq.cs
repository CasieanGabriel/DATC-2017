using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator
{
    class RecievedMessage
    {
        string url = "amqp://rqbbmdli:5jFyVsm3x3JiNLOB9nO7WlIzJqvQmqnk@donkey.rmq.cloudamqp.com/rqbbmdli";

        ConnectionFactory connFactory = new ConnectionFactory();

        public string Get()
        {
            connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var queueName = "ti-am trimis";

                var data = channel.BasicGet(queueName, false);
                if (data == null) return null;

                var message = Encoding.UTF8.GetString(data.Body);

                channel.BasicAck(data.DeliveryTag, false);
                return message.ToString();
            }
        }
    }
}
