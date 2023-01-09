using Assignment4_Hearthstone.Models;
using Assignment4_Hearthstone.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4_Hearthstone.Controllers
{
    [Route("types")]
    [ApiController]
    public class CardTypesController : ControllerBase
    {
        private readonly CardTypeService _typeService;
        private readonly ILogger<CardTypesController> _logger;

        public CardTypesController(CardTypeService typeService, ILogger<CardTypesController> logger)
        {
            _typeService = typeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CardType>> GetCardTypes()
        {
            _logger.LogInformation("Get all card types");

            var cardtype = await _typeService.GetAsync();

            if (cardtype == null)
                return NotFound();

            return Ok(cardtype);
        }

        [HttpPost]
        public ActionResult SeedData()
        {
            _typeService.CreateCardTypes();
            return Ok();
        }
    }
}
