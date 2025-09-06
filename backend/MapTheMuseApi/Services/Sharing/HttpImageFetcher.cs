using Microsoft.Extensions.Caching.Memory;
using SkiaSharp;

public interface IImageFetcher {
    Task<SKImage?> FetchAsSkImageAsync(string url, CancellationToken ct = default);
}

public class HttpImageFetcher : IImageFetcher {
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    public HttpImageFetcher(HttpClient http, IMemoryCache cache) {
        _http = http; _cache = cache;
    }

    public async Task<SKImage?> FetchAsSkImageAsync(string url, CancellationToken ct = default) {
        if (_cache.TryGetValue(url, out SKImage cached)) return cached;

        using var resp = await _http.GetAsync(url, ct);
        if (!resp.IsSuccessStatusCode) return null;
        await using var ms = new MemoryStream();
        await resp.Content.CopyToAsync(ms, ct);
        ms.Position = 0;
        using var data = SKData.Create(ms);
        var img = SKImage.FromEncodedData(data);
        if (img is null) return null;

        _cache.Set(url, img, TimeSpan.FromHours(6));
        return img;
    }
}
