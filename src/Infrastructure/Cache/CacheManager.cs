
using Serilog;

namespace Infrastructure.Cache;
public interface ICacheProvider
{
    void Set(string key, object value, TimeSpan expirationTime);
    object Get(string key);
    bool Contains(string key);
    void Remove(string key);
    void Clear();
}

public class InMemoryCacheProvider : ICacheProvider
{
    private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
    private readonly Dictionary<string, DateTime> _expirationTimes = new Dictionary<string, DateTime>();
    private readonly object _lock = new object();

    public void Set(string key, object value, TimeSpan expirationTime)
    {
        lock (_lock)
        {
            _cache[key] = value;
            _expirationTimes[key] = DateTime.Now.Add(expirationTime);
        }
    }
   
    public object Get(string key)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                if (_expirationTimes[key] > DateTime.Now)
                {
                    return value;
                }
                else
                {
                    _cache.Remove(key);
                    _expirationTimes.Remove(key);
                }
            }
            return null;
        }
    }

    public bool Contains(string key)
    {
        lock (_lock)
        {
            return _cache.ContainsKey(key) && _expirationTimes[key] > DateTime.Now;
        }
    }

    public void Remove(string key)
    {
        lock (_lock)
        {
            _cache.Remove(key);
            _expirationTimes.Remove(key);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _cache.Clear();
            _expirationTimes.Clear();
        }
    }
}

public class RedisCacheProvider : ICacheProvider
{
    // Burada Redis'e özgü kodlar olmalı, örnek olması açısından gerçek kodlar bulunmuyor.
    // Redis bağlantısı ve işlemleri için uygun bir kütüphane kullanılmalıdır.
    public void Set(string key, object value, TimeSpan expirationTime)
    {
        throw new NotImplementedException();
    }
   
    public object Get(string key)
    {
        throw new NotImplementedException();
    }

    public bool Contains(string key)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }
}

public class CacheManager
{
    private readonly ICacheProvider _cacheProvider;

    public CacheManager(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public void Set(string key, object value, TimeSpan expirationTime)
    {
        _cacheProvider.Set(key, value, expirationTime);
        Log.Information("_cacheProvider.Set " + key);

    }

    public object Get(string key)
    {
        Log.Information("_cacheProvider.Set " + key);
        return _cacheProvider.Get(key);
    }

    public bool Contains(string key)
    {
        Log.Information("_cacheProvider.Set " + key);
        return _cacheProvider.Contains(key);
    }

    public void Remove(string key)
    {
        _cacheProvider.Remove(key);
    }

    public void Clear()
    {
        _cacheProvider.Clear();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Örnek kullanım
        ICacheProvider cacheProvider = new InMemoryCacheProvider(); // RedisCacheProvider() olarak değiştirilebilir
        CacheManager cacheManager = new CacheManager(cacheProvider);

        // Örnek veri ekleyelim
        cacheManager.Set("myKey", "myValue", TimeSpan.FromSeconds(10));

        // Örnek veriyi alalım
        var cachedData = cacheManager.Get("myKey");
        Console.WriteLine(cachedData);

        // Cache'den veriyi silmek
        cacheManager.Remove("myKey");

        // Belleği temizlemek
        cacheManager.Clear();
    }
}