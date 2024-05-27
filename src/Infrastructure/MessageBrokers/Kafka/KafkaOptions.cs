namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public Dictionary<string, string> Topics { get; set; }
}
