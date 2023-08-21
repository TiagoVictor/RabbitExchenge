using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
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
    "ex.topic",
    "topic",
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

channel.QueueDeclare(
    "my.queue3",
    true,
    false,
    false,
    null);

channel.QueueBind("my.queue1","ex.topic", "*.image.*");
channel.QueueBind("my.queue2", "ex.topic", "#.image");
channel.QueueBind("my.queue3", "ex.topic", "image.#");

channel.BasicPublish(
    "ex.topic",
    "convert.image.bmp",
    null,
    Encoding.UTF8.GetBytes("routing key convert.image.bmp"));

channel.BasicPublish(
    "ex.topic",
    "convert.bitmap.image",
    null,
    Encoding.UTF8.GetBytes("routing key convert.bitmap.image"));

channel.BasicPublish(
    "ex.topic",
    "image.bitmap.32bit",
    null,
    Encoding.UTF8.GetBytes("routing key image.bitmap.32bit"));