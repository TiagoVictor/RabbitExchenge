using RabbitMQ.Client;
using System.Text;

IConnection connection;
IModel channel;

ConnectionFactory factory = new ConnectionFactory();

factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

connection = factory.CreateConnection();
channel = connection.CreateModel();

channel.ExchangeDeclare(
    "ex.direct",
    "direct",
    true,
    false,
    null);

channel.QueueDeclare(
    "my.infos",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.errors",
    true,
    false,
    false,
    null);

channel.QueueDeclare(
    "my.warnings",
    true,
    false,
    false,
    null);

channel.QueueBind("my.infos", "ex.direct", "info");
channel.QueueBind("my.warnings", "ex.direct", "warning");
channel.QueueBind("my.errors", "ex.direct", "error");

channel.BasicPublish(
    "ex.direct",
    "info",
    null,
    Encoding.UTF8.GetBytes("My info"));

channel.BasicPublish(
    "ex.direct",
    "error",
    null,
    Encoding.UTF8.GetBytes("My error"));

channel.BasicPublish(
    "ex.direct",
    "warning",
    null,
    Encoding.UTF8.GetBytes("My warning"));