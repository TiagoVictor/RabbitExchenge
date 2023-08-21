using RabbitMQ.Client;
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

channel.ExchangeDeclare(
    "ex.fanout",
    "fanout",
    true,
    false,
    null);

channel.QueueDeclare(
    "my.queue1",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.queue2",
    true,
    false,
    false,
    null);

channel.QueueBind(
    "my.queue1",
    "ex.fanout",
    "");

channel.QueueBind(
    "my.queue2",
    "ex.fanout",
    "");

channel.BasicPublish(
    "ex.fanout",
    "",
    null,
    Encoding.UTF8.GetBytes("Ola mundo"));

channel.BasicPublish(
    "ex.fanout",
    "",
    null,
    Encoding.UTF8.GetBytes("Ola mundo2"));

Console.WriteLine("Press a key to exit");
Console.ReadKey();

channel.QueueDelete("my.queue1");
channel.QueueDelete("my.queue2");
channel.ExchangeDelete("ex.fanout");

channel.Close();
connection.Close();