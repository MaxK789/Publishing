using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Publishing.Core.DTOs;

namespace Publishing.Services;

public class RabbitOrganizationEventsPublisher : IOrganizationEventsPublisher, IDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public event Action<OrganizationDto>? OrganizationUpdated;

    public RabbitOrganizationEventsPublisher(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("organizations", ExchangeType.Topic, durable: true);

        var queue = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue, "organizations", "organization.updated");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var organization = JsonSerializer.Deserialize<OrganizationDto>(message);
            OrganizationUpdated?.Invoke(organization!);
        };
        _channel.BasicConsume(queue, autoAck: true, consumer);
    }

    public void PublishOrganizationUpdated(OrganizationDto organization)
    {
        OrganizationUpdated?.Invoke(organization);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(organization));
        _channel.BasicPublish("organizations", "organization.updated", null, body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
