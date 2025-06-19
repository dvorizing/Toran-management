using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Toran.BL;
using Toran.Models;

namespace Toran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailToToranController : ControllerBase
    {
        private readonly SendMailToToran _sendMailBL;

        public SendMailToToranController(SendMailToToran sendMailBL)
        {
            _sendMailBL = sendMailBL;
        }

        [HttpGet]
        public async Task<ActionResult> SendMailToToran()
        {
            try
            {
                var result = await _sendMailBL.SendMailToNextToran();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }
    }
}
