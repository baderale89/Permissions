using Confluent.Kafka;
using Newtonsoft.Json;
using Permissions.Interfaces;

namespace Permissions.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducer> _logger;
        public KafkaProducer(ProducerConfig config, string topic, ILogger<KafkaProducer> logger)
        {
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = topic;
            _logger = logger;
        }

        public async Task ProduceOperationAsync(string operationName, int? permissionId = null)
        {
            try
            {
                var message = new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(new
                    {
                        Id = Guid.NewGuid(),
                        NameOperation = operationName,
                        PermissionId = permissionId
                    })
                };

                var deliveryResult = await _producer.ProduceAsync(_topic, message);
                _logger.LogInformation("Produced message to {Topic}: {Message}", _topic, message.Value);
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError(ex, "Failed to produce message to Kafka");
                throw;
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
