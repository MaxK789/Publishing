using System;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Publishing.Core.DTOs;

namespace Publishing.Services;

public class RabbitProfileEventsPublisher : IProfileEventsPublisher, IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public event Action<ProfileDto>? ProfileUpdated;

    public RabbitProfileEventsPublisher(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("profiles", ExchangeType.Topic, durable: true);

        var queueName = $"{Environment.GetEnvironmentVariable("SERVICE_NAME")}-profiles";
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, "profiles", "profile.updated");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var profile = JsonSerializer.Deserialize<ProfileDto>(message);
            ProfileUpdated?.Invoke(profile!);
        };
        _channel.BasicConsume(queueName, autoAck: true, consumer);
    }

    public void PublishProfileUpdated(ProfileDto profile)
    {
        ProfileUpdated?.Invoke(profile);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(profile));
        var props = _channel.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.Headers = new Dictionary<string, object>
        {
            ["traceparent"] = Activity.Current?.Id ?? string.Empty
        };
        _channel.BasicPublish("profiles", "profile.updated", props, body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
