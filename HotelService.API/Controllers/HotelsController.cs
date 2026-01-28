using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelService.Application.Interfaces;
using HotelService.Domain;

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
    }
}
