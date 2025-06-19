using Microsoft.AspNetCore.Mvc;
using Toran.Dal;
using Toran.Models;

namespace Toran.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToranController : ControllerBase
    {
        private readonly IToranRepository _repository;

        public ToranController(IToranRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToranModel>>> GetAll()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToranModel>> GetById(int id)
        {
            try
            {
                var toran = await _repository.GetByIdAsync(id);
                if (toran == null) return NotFound();
                return Ok(toran);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ToranModel>> Create(ToranModel toran)
        {
            try
            {
                await _repository.AddAsync(toran);
                return Ok(toran);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ToranModel toran)
        {
            try
            {
                if (id != toran.Id) return BadRequest("ID mismatch");

                var updated = await _repository.UpdateAsync(toran);
                if (!updated) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);
                if (!deleted) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }
    }
}
