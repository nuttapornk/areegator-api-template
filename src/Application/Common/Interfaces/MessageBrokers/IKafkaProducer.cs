namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.MessageBrokers;

public interface IKafkaProducer<T>
{
    Task SendAsync(string key, T message, CancellationToken cancellationToken = default);
}
