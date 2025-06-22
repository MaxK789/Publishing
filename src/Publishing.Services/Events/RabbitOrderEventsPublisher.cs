using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Publishing.Core.DTOs;

namespace Publishing.Services;

public class RabbitOrderEventsPublisher : IOrderEventsPublisher, IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public event Action<OrderDto>? OrderCreated;
    public event Action<OrderDto>? OrderUpdated;

    public RabbitOrderEventsPublisher(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("orders", ExchangeType.Topic, durable: true);

        var queue = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue, "orders", "order.*");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderDto>(message);
            if (e.RoutingKey == "order.created")
                OrderCreated?.Invoke(order!);
            else
                OrderUpdated?.Invoke(order!);
        };
        _channel.BasicConsume(queue, autoAck: true, consumer);
    }

    public void PublishOrderCreated(OrderDto order)
    {
        OrderCreated?.Invoke(order);
        Publish("order.created", order);
    }

    public void PublishOrderUpdated(OrderDto order)
    {
        OrderUpdated?.Invoke(order);
        Publish("order.updated", order);
    }

    private void Publish(string routingKey, OrderDto order)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
        _channel.BasicPublish("orders", routingKey, null, body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
