using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.MessageBrokers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers;

public static class MessageBrokersCollectionExtensions
{
    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services,
    KafkaOptions options)
    {
        services.AddKafkaProducer<T>(options);
        return services;
    }

    private static IServiceCollection AddKafkaProducer<T>(this IServiceCollection services, KafkaOptions options)
    {
        services.AddSingleton<IKafkaProducer<T>>(new KafkaProducer<T>(options.BootstrapServers,
            options.Topics[typeof(T).Name]));
        return services;
    }

    public static KafkaOptions GetKafkaOption(IConfiguration configuration)
    {
        var kafkaOption = new KafkaOptions();
        configuration.GetSection("KafkaOptions").Bind(kafkaOption);
#if DEBUG
        kafkaOption.BootstrapServers = "mandalorian-dev.ntl.co.th:9092";
#endif
        return kafkaOption;
    }
}
