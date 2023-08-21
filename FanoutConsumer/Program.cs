using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
using System.Text;

IConnection connection;
IModel channel;

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.HostName = "localhost";
connectionFactory.VirtualHost = "/";
connectionFactory.Port = 5672;
connectionFactory.UserName = "guest";
connectionFactory.Password = "guest";

connection = connectionFactory.CreateConnection();
channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    string message = Encoding.UTF8.GetString(args.Body.Span);
    Console.WriteLine("Message: " + message);
    
    
    channel.BasicAck(args.DeliveryTag, false); // Reconhece e não faz o requeu

    // channel.BasicNack(args.DeliveryTag, false, true); Não reconhece mas faz o requeue
    channel.BasicNack(args.DeliveryTag, false, false); // Não reconhece e não faz o requeu
};

var consumerTag = channel.BasicConsume("my.queue1", false, consumer);

Console.WriteLine("Waiting for messages. Press any key to exite");
Console.ReadKey();