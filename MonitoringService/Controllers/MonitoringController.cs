using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringService.DTOs;
using MonitoringService.Services;

namespace MonitoringService.Controllers
{
    [ApiController]
    [Route("api/monitoring")]
    public class MonitoringController : ControllerBase
    {
        private readonly MonitoringService.Services.MonitoringService _monitoringService;

        public MonitoringController(MonitoringService.Services.MonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        /// <summary>
        /// GET /api/monitoring/consumption?deviceId=...&date=YYYY-MM-DD
        /// </summary>
        [HttpGet("consumption")]
        [Authorize]
        public async Task<ActionResult<List<HourlyConsumptionDto>>> GetDailyConsumption(
            [FromQuery] Guid deviceId,
            [FromQuery] DateOnly date)
        {
            var result = await _monitoringService.GetDailyConsumptionAsync(deviceId, date);
            return Ok(result);
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok("OK");
        }
    }
}
