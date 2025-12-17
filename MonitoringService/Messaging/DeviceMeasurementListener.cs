using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonitoringService.DTOs;
using MonitoringService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MonitoringService.Messaging
{
    public class DeviceMeasurementListener : BackgroundService
    {
        private const string QueueName = "device_to_monitoring_queue";

        private readonly ILogger<DeviceMeasurementListener> _logger;
        private readonly MeasurementProcessingService _service;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public DeviceMeasurementListener(
            IConnectionFactory factory,
            MeasurementProcessingService service,
            ILogger<DeviceMeasurementListener> logger)
        {
            _logger = logger;
            _service = service;

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<DeviceMeasurementMessage>(json);

                    if (message != null)
                    {
                        _logger.LogInformation("📥 Received measurement: {msg}", json);
                        await _service.ProcessMeasurementAsync(message);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing RabbitMQ message");
                }
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
