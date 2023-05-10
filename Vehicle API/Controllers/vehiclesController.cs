using Vehicle_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_API.Data;

namespace Vehicle_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vehiclesController : ControllerBase
    {
        IRepository Repository { get; set; } 
        ILogger<vehiclesController> Logger;
        public vehiclesController(IRepository repository,ILogger<vehiclesController> logger)
        {
            Repository = repository;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vehicles = await Repository.GetAll();
            Logger.LogInformation($"Retrieved {vehicles.Count()} vehicles");
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vehicle = await Repository.GetFirstOrDefault(v => v.Id == id);
            if (vehicle == null)
            {
                Logger.LogInformation($"Vehicle with id {id} not found");
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            await Repository.Add(vehicle);
            await Repository.Save();
            Logger.LogInformation($"Added vehicle with id {vehicle.Id}");
            return CreatedAtAction(nameof(Get), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            if (id != vehicle.Id)
            {
                Logger.LogInformation($"Vehicle id {id} does not match vehicle id {vehicle.Id}");
                return BadRequest();
            }

            if(!await Repository.CheckVehicleExists(id))
            {
                Logger.LogInformation($"Vehicle with id {id} not found");
                return NotFound();
            }

                Repository.UpdateEntity(vehicle);
                await Repository.Save();
            

            Logger.LogInformation($"Updated vehicle with id {vehicle.Id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await Repository.CheckVehicleExists(id))
            {
                Logger.LogInformation($"Vehicle with id {id} not found");
                return NotFound();
            }
            var vehicle = await Repository.GetFirstOrDefault(v => v.Id == id);
            Repository.Remove(vehicle);
            await Repository.Save();
            Logger.LogInformation($"Deleted vehicle with id {id}");
            return NoContent();
        }
    }
}
