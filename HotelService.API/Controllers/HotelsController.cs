using HotelService.Application.Interfaces;
using HotelService.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _repo;

        public HotelsController(IHotelRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            return hotel == null ? NotFound() : Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Hotel hotel)
        {
            await _repo.AddAsync(hotel);
            return CreatedAtAction(nameof(Get), new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Hotel hotel)
        {
            if (id != hotel.Id) return BadRequest();
            await _repo.UpdateAsync(hotel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{hotelId}/availability")]
        public async Task<IActionResult> CheckAvailability(
    int hotelId,
    DateTime checkIn,
    DateTime checkOut)
        {
            var available = await _repo.CheckAvailabilityAsync(hotelId, checkIn, checkOut);

            return Ok(available);
        }

        [HttpPost("{hotelId}/reserve")]
        public async Task<IActionResult> Reserve(
    int hotelId,
    DateTime checkIn,
    DateTime checkOut)
        {
            await _repo.ReserveRoomsAsync(hotelId, checkIn, checkOut);
            return Ok();
        }

        [HttpPost("{hotelId}/restore")]
        public async Task<IActionResult> Restore(
    int hotelId,
    DateTime checkIn,
    DateTime checkOut)
        {
            await _repo.RestoreRoomsAsync(hotelId, checkIn, checkOut);
            return Ok();
        }

    }
}
