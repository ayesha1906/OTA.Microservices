using System.Net.Http.Headers;

public class AmadeusHotelService
{
    private readonly HttpClient _httpClient;
    private readonly AmadeusAuthService _authService;

    public AmadeusHotelService(HttpClient httpClient, AmadeusAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<string> SearchHotels(string cityCode)
    {
        var token = await _authService.GetAccessToken();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/v2/shopping/hotel-offers?cityCode={cityCode}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
