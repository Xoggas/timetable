namespace Timetable.Frontend.Shared.Services;

public sealed class ApiUrlService
{
    private readonly IConfiguration _configuration;

    public ApiUrlService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Retrieve(string url, string connectionStringName = "ApiUrl")
    {
        var baseUrl = _configuration.GetConnectionString(connectionStringName)!;
        
        return new Uri(new Uri(baseUrl), url).ToString();
    }
}