using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace _3G_webjob.Controller
{
    class RabbitMQMsg
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
                var routingKey = "am_primit";
                channel.BasicPublish(exchangeName, routingKey, null, data);
            }

        }

        public string Get()
        {
            connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var queueName = "ti-am_trimis";    

                var data = channel.BasicGet(queueName, false);
                if (data == null) return null;

                var message = Encoding.UTF8.GetString(data.Body);

                channel.BasicAck(data.DeliveryTag, false);
                return message.ToString();
            }
        }


    }
}
