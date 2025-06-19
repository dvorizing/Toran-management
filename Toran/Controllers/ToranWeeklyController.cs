using Microsoft.AspNetCore.Mvc;
using Toran.BL;

namespace Toran.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToranWeeklyController : ControllerBase
    {
        private readonly ToranDutyCalculator _calculator;

        public ToranWeeklyController(ToranDutyCalculator calculator)
        {
            _calculator = calculator;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetToranForDate(DateTime date)
        {
            var result = await _calculator.GetToranForDateAsync(date);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetToranSchedule(DateTime fromDate, DateTime toDate, string? name = null)
        {
            var list = await _calculator.GetToranDatesForRangeAsync(fromDate, toDate, name);
            return Ok(list.Select(x => new { Date = x.Date.ToString("yyyy-MM-dd"), Toran = x.Name }));
        }

    }
}
