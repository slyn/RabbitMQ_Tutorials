using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleConsumer_01
{
    class Program
    {

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                Password = "isleyen",
                UserName = "isleyen",
                RequestedConnectionTimeout = new TimeSpan(30000)
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                    type: "direct");
                var queueName = channel.QueueDeclare().QueueName;
                var severities = new string[] {"info", "success"};
                foreach (var severity in severities)
                {
                    channel.QueueBind(queue: queueName,
                        exchange: "direct_logs",
                        routingKey: severity);
                }

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                        routingKey, message);
                };
                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        // static void Main(string[] args)
        // {
        //     var factory = new ConnectionFactory()
        //     {
        //         HostName = "localhost",
        //         Port = 5672,
        //         Password = "isleyen",
        //         UserName = "isleyen",
        //         RequestedConnectionTimeout = new TimeSpan(30000)
        //     };
        //     using(var connection = factory.CreateConnection())
        //     using(var channel = connection.CreateModel())
        //     {
        //         channel.QueueDeclare(queue: "hello",
        //             durable: false,
        //             exclusive: false,
        //             autoDelete: false,
        //             arguments: null);
        //
        //         var consumer = new EventingBasicConsumer(channel);
        //         consumer.Received += (model, ea) =>
        //         {
        //             var body = ea.Body.ToArray();
        //             var message = Encoding.UTF8.GetString(body);
        //             Console.WriteLine(" [x] Received {0}", message);
        //         };
        //         channel.BasicConsume(queue: "hello",
        //             autoAck: true,
        //             consumer: consumer);
        //
        //         Console.WriteLine(" Press [enter] to exit.");
        //         Console.ReadLine();
        //     }
        // }
    }
}