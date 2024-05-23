using System.Text;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Caching;

public class RedisCacheService : IRedisCacheService
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ContractResolver = new PrivateSetterContractResolver()
    };

    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    private static byte[] ConvertToBytes(object obj)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, JsonSettings));
    }

    private static T ConvertToObject<T>(byte[] bytes)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
    }


    public T Get<T>(string key) where T : class
    {
        var bytes = _cache.Get(key);
        return bytes == null ? default : ConvertToObject<T>(bytes);
    }

    public byte[] GetBytes(string key)
    {
        return _cache.Get(key);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void Set<T>(string key, T obj, TimeSpan duration) where T : class
    {
        _cache.Set(key, ConvertToBytes(obj), new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = duration
        });
    }

    public void Set(string key, byte[] values, TimeSpan duration)
    {
        _cache.Set(key, values, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = duration
        });
    }

    private class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as System.Reflection.PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}
