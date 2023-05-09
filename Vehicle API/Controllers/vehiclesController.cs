using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_API.Data;

namespace Maintenance_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vehiclesController : ControllerBase
    {
        IRepository Repository { get; set; } 
        public vehiclesController(IRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vehicles = await Repository.GetAll();
            return Ok(vehicles);
        }
    }
}
