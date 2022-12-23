using Assignment4_Hearthstone.Models;
using Assignment4_Hearthstone.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4_Hearthstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly SetService _setService;
        private readonly ILogger<SetsController> _logger;

        public SetsController(SetService setService, ILogger<SetsController> logger)
        {
            _setService = setService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Set>> GetSets()
        {
            _logger.LogInformation("Get all sets");

            var sets = await _setService.GetAsync();

            if (sets == null)
                return NotFound();

            return Ok(sets);
        }

        [HttpPost]
        public ActionResult SeedData()
        {
            _setService.CreateSets();
            return Ok();
        }
    }
}
