namespace SoftPlcPortal.Infrastructure.SoftPlc;

public class SoftPlcClientFactory(IHttpClientFactory httpClientFactory)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public SoftPlcClient Create(string baseUrl)
    {
        var httpClient = _httpClientFactory.CreateClient(baseUrl);
        return new SoftPlcClient(httpClient);
    }
}
