using Assignment4_Hearthstone.Models;
using Assignment4_Hearthstone.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4_Hearthstone.Controllers
{
    [Route("rarities")]
    [ApiController]
    public class RaritiesController : ControllerBase
    {
        private readonly RarityService _rarityService;
        private readonly ILogger<RaritiesController> _logger;

        public RaritiesController(RarityService rarityService, ILogger<RaritiesController> logger)
        {
            _rarityService = rarityService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Rarity>> GetRarities()
        {
            _logger.LogInformation("Get all rarities");

            var rarity = await _rarityService.GetAsync();

            if (rarity == null)
                return NotFound();

            return Ok(rarity);
        }

        [HttpPost]
        public ActionResult SeedData()
        {
            _rarityService.CreateRarities();
            return Ok();
        }
    }
}
