using Microsoft.AspNetCore.Mvc;
using Toran.Dal;
using Toran.Models;

namespace Toran.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToranStatusController : ControllerBase
    {
        private readonly IToranStatusRepository _repository;

        public ToranStatusController(IToranStatusRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToranStatus>>> GetAll()
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
        public async Task<ActionResult<ToranStatus>> GetById(int id)
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
        public async Task<ActionResult<ToranStatus>> Create(ToranStatus toranStatus)
        {
            try
            {
                await _repository.AddAsync(toranStatus);
                return Ok(toranStatus);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ToranStatus toranStatus)
        {
            try
            {
                if (id != toranStatus.Id) return BadRequest("ID mismatch");

                var updated = await _repository.UpdateAsync(toranStatus);
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
