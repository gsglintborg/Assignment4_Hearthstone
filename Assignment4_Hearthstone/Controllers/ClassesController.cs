using Assignment4_Hearthstone.Services;
using Assignment4_Hearthstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4_Hearthstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ClassService _classService;
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(ClassService classService, ILogger<ClassesController> logger)
        {
            _classService = classService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Class>> GetClasses()
        {
            _logger.LogInformation("Get all classes");

            var classes = await _classService.GetAsync();

            if (classes == null)
                return NotFound();

            return Ok(classes);
        }

        [HttpPost]
        public ActionResult SeedData()
        {
            _classService.CreateClasses();
            return Ok();
        }
    }
}
