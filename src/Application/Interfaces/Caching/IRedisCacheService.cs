namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Interfaces.Caching;

public interface IRedisCacheService
{
    T Get<T>(string key) where T : class;

    byte[] GetBytes(string key);

    void Set<T>(string key, T obj, TimeSpan duration) where T : class;

    void Set(string key, byte[] values, TimeSpan duration);

    void Remove(string key);
}
