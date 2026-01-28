using Azure.Core.Pipeline;
using BookingService.Application.Interfaces;
using BookingService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingController(
            IBookingRepository repository,
            IHttpClientFactory httpClientFactory)
        {
            this._repository = repository;
            this._httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            // Validate Hotel exists
            var client = _httpClientFactory.CreateClient("HotelService");
            var response = await client.GetAsync($"api/hotels/{booking.HotelId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Invalid HotelId");

            var created = await _repository.CreateAsync(booking);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            return booking == null ? NotFound() : Ok(booking);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _repository.CancelAsync(id);
            return NoContent();
        }
    }
}
