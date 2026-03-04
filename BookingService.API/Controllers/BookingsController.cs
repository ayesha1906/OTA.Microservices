using Azure.Core.Pipeline;
using BookingService.Application.Interfaces;
using BookingService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [ApiController]
    [Route("api/[controlLer]")]
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
        public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
        {
            var client = _httpClientFactory.CreateClient("HotelService");

            // Validate hotel & fetch hotel details (including price)
            var hotelResponse = await client.GetAsync($"api/hotels/{dto.HotelId}");

            if (!hotelResponse.IsSuccessStatusCode)
                return BadRequest("Invalid Hotel");

            var hotel = await hotelResponse.Content.ReadFromJsonAsync<HotelDto>();

            if (hotel == null)
                return BadRequest("Hotel data not found");

            // Validate dates
            var nights = (dto.CheckOut - dto.CheckIn).Days;

            if (nights <= 0)
                return BadRequest("Invalid date range");

            // Check availability BEFORE creating booking
            var availabilityResponse = await client.GetAsync(
                $"api/hotels/{dto.HotelId}/availability?checkIn={dto.CheckIn:O}&checkOut={dto.CheckOut:O}");

            if (!availabilityResponse.IsSuccessStatusCode)
                return BadRequest("Error checking availability");

            var isAvailable = await availabilityResponse.Content.ReadFromJsonAsync<bool>();

            if (!isAvailable)
                return BadRequest("Rooms not available");

            // Calculate total price using PricePerNight from HotelService
            var totalPrice = nights * hotel.PricePerNight;

            // Create booking with PENDING_PAYMENT status
            var booking = new Booking
            {
                HotelId = dto.HotelId,
                UserEmail = dto.UserEmail,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = totalPrice,
                Status = "PENDING_PAYMENT",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(booking);

            // Simulate payment
            await Task.Delay(1500);
            var paymentSuccess = true; // Random.Shared.Next(1, 10) > 2;// rnadomly deciding

            if (!paymentSuccess)
            {
                booking.Status = "FAILED";
                await _repository.UpdateAsync(booking);
                return BadRequest("Payment Failed");
            }

            // Reserve rooms in HotelService
            var reserveResponse = await client.PostAsync(
                $"api/hotels/{dto.HotelId}/reserve?checkIn={dto.CheckIn:O}&checkOut={dto.CheckOut:O}",
                null);

            if (!reserveResponse.IsSuccessStatusCode)
            {
                booking.Status = "FAILED";
                await _repository.UpdateAsync(booking);
                return BadRequest("Failed to reserve rooms");
            }

            // Confirm booking
            booking.Status = "CONFIRMED";
            await _repository.UpdateAsync(booking);

            return Ok(booking);
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
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
                return NotFound();

            if (booking.Status != "CONFIRMED")
                return BadRequest("Only confirmed bookings can be cancelled");

            var hoursBeforeCheckIn = (booking.CheckIn - DateTime.UtcNow).TotalHours;

            decimal refund = 0;

            if (hoursBeforeCheckIn > 24)
            {
                refund = booking.TotalPrice; // full refund
            }
            else if (hoursBeforeCheckIn > 0)
            {
                refund = booking.TotalPrice * 0.5m; // 50% refund
            }
            else
            {
                refund = 0; // no refund
            }

            booking.Status = "CANCELLED";
            booking.RefundAmount = refund;

            var client = _httpClientFactory.CreateClient("HotelService");

            await client.PostAsync(
                $"api/hotels/{booking.HotelId}/restore?checkIn={booking.CheckIn:O}&checkOut={booking.CheckOut:O}",
                null);

            await _repository.UpdateAsync(booking);

            return Ok(new
            {
                Message = "Booking Cancelled",
                RefundAmount = refund
            });
        }

        [HttpGet("user/{email}")]

        public async Task<IActionResult> GetUserBookings(string email)
        {
            var bookings = await _repository.GetByUserAsync(email);
            return Ok(bookings);
        }
    }
}
