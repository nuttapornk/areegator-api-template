using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.MessageBrokers;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Net;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;

public class KafkaProducer<T> : IKafkaProducer<T>, IDisposable
{
    private const int DefaultMessageSize = 1024 * 1024 * 10;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public KafkaProducer(string bootstrapServers, string topic)
    {
        _topic = topic;
        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            ClientId = Dns.GetHostName(),
            MessageMaxBytes = DefaultMessageSize
        }).Build();
    }

    public async Task SendAsync(string key, T message, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = JsonConvert.SerializeObject(message) }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}
