namespace SoftPlcPortal.Infrastructure.SoftPlc;

public sealed class SoftPlcClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    private static readonly StringContent s_emptyStringContent = new(string.Empty);

    public async Task<SoftPlcDataBlockResponse[]> GetAllDataBlocksAsync(CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/DataBlocks";
        var content = await _httpClient.GetFromJsonAsync<SoftPlcDataBlockResponse[]>(requestUri, cancellationToken) ?? [];
        return content;
    }

    public async Task CreateDataBlockAsync(int id, int size, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/DataBlocks?id={id}&size={size}";
        var response = await _httpClient.PostAsync(requestUri, s_emptyStringContent, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDataBlockAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/DataBlocks/{id}";
        var response = await _httpClient.DeleteAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<SoftPlcDataBlockResponse> GetDataBlockByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/DataBlocks/{id}";
        var content = await _httpClient.GetFromJsonAsync<SoftPlcDataBlockResponse>(requestUri, cancellationToken);
        return content!;
    }

    public async Task SetDataBlockAsync(int id, string data, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/DataBlocks/{id}";
        var response = await _httpClient.PutAsync(requestUri, new StringContent(data), cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}

public record SoftPlcDataBlockResponse(int Id, int Size, string Data);