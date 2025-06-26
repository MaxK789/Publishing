using System;
using System.Text;
using System.Text.Json;
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

        var queue = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue, "profiles", "profile.updated");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var profile = JsonSerializer.Deserialize<ProfileDto>(message);
            ProfileUpdated?.Invoke(profile!);
        };
        _channel.BasicConsume(queue, autoAck: true, consumer);
    }

    public void PublishProfileUpdated(ProfileDto profile)
    {
        ProfileUpdated?.Invoke(profile);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(profile));
        _channel.BasicPublish("profiles", "profile.updated", null, body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
