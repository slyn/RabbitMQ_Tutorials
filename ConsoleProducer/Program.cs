using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace ConsoleProducer
{
    class Program
    {
        // docker run -d --hostname my-rabbit --name qrabbit -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=isleyen -e RABBITMQ_DEFAULT_PASS=isleyen rabbitmq:3-management

        #region HelloWorld

        // static void Main(string[] args)
        // {
        //     var factory = new ConnectionFactory() {                 
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
        //         string message = "Hello World!";
        //         var body = Encoding.UTF8.GetBytes(message);
        //
        //         channel.BasicPublish(exchange: "",
        //             routingKey: "hello",
        //             basicProperties: null,
        //             body: body);
        //         Console.WriteLine(" [x] Sent {0}", message);
        //     }
        //
        //     Console.WriteLine(" Press [enter] to exit.");
        //     Console.ReadLine();
        // }
        //

        #endregion

        #region WorkerQueue

        // static void Main(string[] args)
        // {
        //     var factory = new ConnectionFactory()
        //     {
        //         HostName = "localhost",
        //         Port = 5672,
        //         Password = "isleyen",
        //         UserName = "isleyen",
        //         RequestedConnectionTimeout = new TimeSpan(30000) 
        //         
        //     };
        //     using(var connection = factory.CreateConnection())
        //     using(var channel = connection.CreateModel())
        //     {
        //         channel.QueueDeclare(queue: "task_queue",
        //             durable: true,
        //             exclusive: false,
        //             autoDelete: false,
        //             arguments: null);
        //
        //         int x = 0;
        //         var rd = new Random();
        //         while (true)
        //         {
        //             if (x == 100)
        //             {
        //                 break;
        //             }
        //
        //             x++;
        //             var dotcount = rd.Next(1, 100);
        //             var dots = new String('.', dotcount);
        //             var message = $"Merhaba Mesajı Index {x} _ dotCount {dotcount}: {dots}";
        //             var body = Encoding.UTF8.GetBytes(message);
        //
        //             var properties = channel.CreateBasicProperties();
        //             properties.Persistent = true;
        //
        //             channel.BasicPublish(exchange: "",
        //                 routingKey: "task_queue",
        //                 basicProperties: properties,
        //                 body: body);
        //             Console.WriteLine(" [x] Sent {0}", message);
        //         }
        //        
        //     }
        //
        //     Console.WriteLine(" Press [enter] to exit.");
        //     Console.ReadLine();
        // }

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
        //         int x = 0;
        //         var rd = new Random();
        //         while (true)
        //         {
        //             if (x == 100)
        //             {
        //                 break;
        //             }
        //
        //             x++;
        //             Thread.Sleep(7000);
        //             var randomSayi = rd.Next(1, 100);
        //
        //             var message = $"{x} - Log Message For {randomSayi}";
        //             var body = Encoding.UTF8.GetBytes(message);
        //             channel.BasicPublish(exchange: "logs",
        //                 routingKey: "",
        //                 basicProperties: null,
        //                 body: body);
        //             Console.WriteLine(" [x] Sent {0}", message);
        //         }
        //     }
        //
        //     Console.WriteLine(" Press [enter] to exit.");
        //     Console.ReadLine();
        // }

        #endregion

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
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
                var severities = new string[] {"error", "info", "warning","success","fail","pending","stopped","waiting"};
                int x = 0;
                var rd = new Random();
                while (true)
                {
                    if (x == 100)
                    {
                        break;
                    }
        
                    x++;
                    var g = rd.Next(1, 1000);
                    var random = g % 8;
                    Thread.Sleep(5000);
                    var severity = severities[random];
        
                    var message = $"{x} - N:{g} : Severity : {severity} : Log mesajı! ";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "direct_logs",
                        routingKey: severity,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
                }
            }
        
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}