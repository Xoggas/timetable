namespace Timetable.Frontend.Shared.Services;

public sealed class ApiUrlService
{
    private readonly string _baseApiUrl;

    public ApiUrlService(IConfiguration configuration)
    {
        _baseApiUrl = configuration.GetConnectionString("ApiUrl") ??
                      throw new ArgumentException("Api Url not configured.");
    }

    public string Retrieve(string url)
    {
        return new Uri(new Uri(_baseApiUrl), url).ToString();
    }
}