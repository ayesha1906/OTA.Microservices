using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using SearchService.API.Models;
using SearchService.API.Services;
using System.Text.Json;

namespace SearchService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICacheService _cacheService;

        public SearchController(IHttpClientFactory httpClientFactory, ICacheService cacheService)
        {
            this._httpClientFactory = httpClientFactory;
            this._cacheService = cacheService;
        }

        [HttpGet("hotels")]
        public async Task<IActionResult> SearchHotels(string city)
        {
            //var client = _httpClientFactory.CreateClient("HotelService");

            //var response = await client.GetAsync("api/hotels");

            //if (!response.IsSuccessStatusCode)
            //    return StatusCode((int)response.StatusCode, "Hotel service unavailable");

            //var json = await response.Content.ReadAsStringAsync();

            //var hotels = JsonSerializer.Deserialize<List<HotelDto>>(
            //    json,
            //    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            //);

            //var filtered = hotels!
            //    .Where(h => h.City.Equals(city, StringComparison.OrdinalIgnoreCase))
            //    .ToList();

            //return Ok(filtered);

            var cacheKey = $"hotels_{city.ToLower()}";

            // cache
            var cachedResult = await _cacheService.GetAsync<List<HotelDto>>(cacheKey);
            if (cachedResult != null)
            {
                return Ok(cachedResult);
            }

            // else Call HotelService
            var client = _httpClientFactory.CreateClient("HotelService");
            try
            {
                var response = await client.GetFromJsonAsync<List<HotelDto>>("/api/Hotels");

                var filtered = response!
                    .Where(h => h.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                //  Store in cache
                await _cacheService.SetAsync(cacheKey, filtered, TimeSpan.FromMinutes(5));

                return Ok(filtered);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(503, "Hotel service temporarily unavailable. Please try again shortly.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //return Ok(new List<HotelDto>()); // graceful fallback - never crashes
            }
        }

        [HttpGet("amadeus/hotels")]
        public async Task<IActionResult> GetFromAmadeus([FromQuery] string cityCode, [FromServices] AmadeusHotelService service)
        {
            var result = await service.SearchHotels(cityCode);
            return Ok(result);
        }


    }
}