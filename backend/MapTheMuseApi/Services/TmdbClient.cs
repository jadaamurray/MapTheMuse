using System.Net.Http.Json;

public class TmdbClient
{
    private readonly HttpClient _http;
    private readonly string _lang;
    private const string ImgBase = "https://image.tmdb.org/t/p/w500";

    public TmdbClient(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _http.BaseAddress ??= new Uri(cfg["TMDB:BaseUrl"] ?? "https://api.themoviedb.org/3/");
        _lang = cfg["TMDB:DefaultLanguage"] ?? "en-GB";
    }

    public async Task<(string title, int? year, string? creator, string? poster, string? overview)>
        GetMovieAsync(string id, string? language = null)
    {
        var res = await _http.GetFromJsonAsync<MovieDto>($"movie/{id}?language={(language ?? _lang)}");
        if (res is null) return (id, null, null, null, null);

        var year = TryYear(res.release_date);
        var poster = string.IsNullOrEmpty(res.poster_path) ? null : $"{ImgBase}{res.poster_path}";
        return (res.title ?? id, year, null, poster, res.overview);
    }

    public async Task<(string title, int? year, string? creator, string? poster, string? overview)>
        GetTvAsync(string id, string? language = null)
    {
        var res = await _http.GetFromJsonAsync<TvDto>($"tv/{id}?language={(language ?? _lang)}");
        if (res is null) return (id, null, null, null, null);

        var year = TryYear(res.first_air_date);
        var poster = string.IsNullOrEmpty(res.poster_path) ? null : $"{ImgBase}{res.poster_path}";
        return (res.name ?? id, year, null, poster, res.overview);
    }

    private static int? TryYear(string? isoDate) =>
        !string.IsNullOrWhiteSpace(isoDate) && DateOnly.TryParse(isoDate, out var d) ? d.Year : null;

    private sealed class MovieDto
    {
        public string? title { get; set; }
        public string? release_date { get; set; }
        public string? poster_path { get; set; }
        public string? overview { get; set; }
    }
    private sealed class TvDto
    {
        public string? name { get; set; }
        public string? first_air_date { get; set; }
        public string? poster_path { get; set; }
        public string? overview { get; set; }
    }
}
