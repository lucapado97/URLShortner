using System.Collections.Concurrent;

namespace URLShortner;

/// <summary>
/// Thread-safe URL storage service that maps 6-digit keys to original URLs.
/// Uses ConcurrentDictionary and Interlocked operations to ensure thread safety.
/// Includes deduplication: the same URL always returns the same key.
/// </summary>
public class UrlStore
{
    private readonly ConcurrentDictionary<string, string> _keysToUrls = new();
    private readonly ConcurrentDictionary<string, string> _urlsToKeys = new();
    private long _lastKey = 0;

    public string AddUrl(string url)
    {
        string key = _urlsToKeys.GetOrAdd(url, (u) =>
        {
            long keyNumber = Interlocked.Increment(ref _lastKey);
            string newKey = keyNumber.ToString("D6");
            _keysToUrls[newKey] = u;
            return newKey;
        });

        return key;
    }

    public string? GetUrl(string key)
    {
        _keysToUrls.TryGetValue(key, out string? url);
        return url;
    }

    public void InitializeSampleUrls()
    {
        AddUrl("https://www.google.com");
        AddUrl("https://www.github.com");
        AddUrl("https://www.stackoverflow.com");
    }
}
