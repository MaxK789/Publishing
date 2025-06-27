using System;
using System.Text;
using System.Diagnostics;
using System.Text.Json;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Polly;
using Publishing.Core.DTOs;

namespace Publishing.Services;

public class RabbitOrderEventsPublisher : IOrderEventsPublisher, IDisposable
{
    private IModel? _channel;
    private IConnection? _connection;

    public event Action<OrderDto>? OrderCreated;
    public event Action<OrderDto>? OrderUpdated;

    public RabbitOrderEventsPublisher(string connectionString)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString),
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
        };

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForever(_ => TimeSpan.FromSeconds(5),
                (ex, _) => Console.WriteLine($"RabbitMQ connection failed: {ex.Message}, retrying..."));

        try
        {
            policy.Execute(() =>
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to connect to RabbitMQ: {ex.Message}");
            return;
        }

        if (_channel is null)
            return;

        _channel.ExchangeDeclare("orders", ExchangeType.Topic, durable: true);

        var queueName = $"{Environment.GetEnvironmentVariable("SERVICE_NAME")}-orders";
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, "orders", "order.*");
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
        _channel.BasicConsume(queueName, autoAck: true, consumer);
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
        if (_channel is null)
            return;

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
        var props = _channel.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.Headers = new Dictionary<string, object>
        {
            ["traceparent"] = Activity.Current?.Id ?? string.Empty
        };
        _channel.BasicPublish("orders", routingKey, props, body);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
