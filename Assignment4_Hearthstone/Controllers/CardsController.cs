using Microsoft.AspNetCore.Mvc;
using Assignment4_Hearthstone.Models;
using Assignment4_Hearthstone.Services;

namespace Assignment4_Hearthstone.Controllers
{
    [Route("cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardService _cardService;
        private readonly ILogger<CardsController> _logger;

        public CardsController(CardService cardService, ILogger<CardsController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CardMappedToMetadataDTO>> GetCardsAsync([FromQuery] QueryParameters param)
        {
            _logger.LogInformation(
                $"Page = {param.Page}\n" +
                $"SetId = {param.SetId}\n" +
                $"Artist = {param.Artist}\n" +
                $"ClassId = {param.ClassId}\n" +
                $"RarityId = {param.RarityId}\n");

            var result = await _cardService.GetCardsByQueryAsync(param);

            _logger.LogInformation($"NumberOfCardsFound = {result.Count}\n");

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Seeding of data is moved to Program.cs, removing the need to manually POST the data
        //[HttpPost]
        //public ActionResult SeedData()
        //{
        //    _cardService.CreateCards();
        //    return Ok();
        //}
    }
}
