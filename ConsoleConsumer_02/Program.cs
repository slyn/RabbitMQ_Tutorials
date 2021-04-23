using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleConsumer_02
{
    class Program
    {
        #region HelloWorld

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
        //             Console.WriteLine("Bekleme başladı");
        //             Thread.Sleep(15000);
        //             Console.WriteLine("Bekleme bitti");
        //         };
        //         channel.BasicConsume(queue: "hello",
        //             autoAck: true,
        //             consumer: consumer);
        //
        //         Console.WriteLine(" Press [enter] to exit.");
        //         Console.ReadLine();
        //     }
        // }
        //
        #endregion

        #region WorkerProcess

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
        //     using (var connection = factory.CreateConnection())
        //     using (var channel = connection.CreateModel())
        //     {
        //         channel.QueueDeclare(queue: "task_queue",
        //             durable: true,
        //             exclusive: false,
        //             autoDelete: false,
        //             arguments: null);
        //
        //         channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        //
        //         Console.WriteLine(" [*] Waiting for messages.");
        //
        //         var consumer = new EventingBasicConsumer(channel);
        //         consumer.Received += (sender, ea) =>
        //         {
        //             var body = ea.Body.ToArray();
        //             var message = Encoding.UTF8.GetString(body);
        //             Console.WriteLine(" [x] Received {0}", message);
        //
        //             int dots = message.Split('.').Length - 1;
        //             Thread.Sleep(dots * 1000);
        //
        //             Console.WriteLine(" [x] Done");
        //
        //             // Note: it is possible to access the channel via
        //             //       ((EventingBasicConsumer)sender).Model here
        //             channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        //         };
        //         channel.BasicConsume(queue: "task_queue",
        //             autoAck: false,
        //             consumer: consumer);
        //
        //         Console.WriteLine(" Press [enter] to exit.");
        //         Console.ReadLine();
        //     }
        //}


        #endregion

        #region PubSub

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
        //         channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
        //
        //         var queueName = channel.QueueDeclare().QueueName;
        //         channel.QueueBind(queue: queueName,
        //             exchange: "logs",
        //             routingKey: "");
        //
        //         Console.WriteLine(" [*] Waiting for logs.");
        //
        //         var consumer = new EventingBasicConsumer(channel);
        //         consumer.Received += (model, ea) =>
        //         {
        //             var body = ea.Body.ToArray();
        //             var message = Encoding.UTF8.GetString(body);
        //             Console.WriteLine(" [x] {0}", message);
        //         };
        //         channel.BasicConsume(queue: queueName,
        //             autoAck: true,
        //             consumer: consumer);
        //
        //         Console.WriteLine(" Press [enter] to exit.");
        //         Console.ReadLine();
        //     }
        // }

        #endregion

        #region MyRegion

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
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                    type: "direct");
                var queueName = channel.QueueDeclare().QueueName;
                var severities = new string[] {"error", "warning"};
                foreach(var severity in severities)
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
        
        
        #endregion
    }
}