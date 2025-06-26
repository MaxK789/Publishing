using System;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
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

        var queueName = $"{Environment.GetEnvironmentVariable("SERVICE_NAME")}-organizations";
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, "organizations", "organization.updated");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (s, e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var organization = JsonSerializer.Deserialize<OrganizationDto>(message);
            OrganizationUpdated?.Invoke(organization!);
        };
        _channel.BasicConsume(queueName, autoAck: true, consumer);
    }

    public void PublishOrganizationUpdated(OrganizationDto organization)
    {
        OrganizationUpdated?.Invoke(organization);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(organization));
        var props = _channel.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.Headers = new Dictionary<string, object>
        {
            ["traceparent"] = Activity.Current?.Id ?? string.Empty
        };
        _channel.BasicPublish("organizations", "organization.updated", props, body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
