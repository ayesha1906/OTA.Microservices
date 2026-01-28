using SearchService.API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

public class AmadeusAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public AmadeusAuthService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetAccessToken()
    {
        var clientId = _config["Amadeus:ClientId"];
        var clientSecret = _config["Amadeus:ClientSecret"];

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/v1/security/oauth2/token");

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId },
            { "client_secret", clientSecret }
        });

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<Amadeus>(json);

        return token.access_token;
    }
}
