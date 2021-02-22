using RabbitMQ.Client;
using System;
using System.Text;

namespace EGTDigital.RabbitMq
{
    public class MessageSender
    {
        private string exchangeName;

        public MessageSender(string exchange)
        {
            this.exchangeName = exchange;
        }

        public void SendMessage(string jsonMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(exchange: this.exchangeName,
                                     routingKey: "test",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
