using Vehicle_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_API.Data;
using AutoMapper;
using Vehicle_API.DTO;

namespace Vehicle_API.Controllers
{
    /// <summary>
    /// Controller for vehicles
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class vehiclesController : ControllerBase
    {
        IRepository Repository { get; set; } 
        ILogger<vehiclesController> Logger;
        private IMapper Mapper
        {
            get;
        }
        public vehiclesController(IRepository repository,ILogger<vehiclesController> logger,IMapper mapper)
        {
            Repository = repository;
            Logger = logger;
            Mapper = mapper;
        }
        /// <summary>
        /// returns list of all vehicles
        /// </summary>

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vehicle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var vehicles = await Repository.GetAll();
            Logger.LogInformation($"Retrieved {vehicles.Count()} vehicles");
            return Ok(vehicles);
        }
        /// <summary>
        /// returns list of a vehicle by id
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        [HttpGet("{vehicleid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Vehicle),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int vehicleid)
        {
            var vehicle = await Repository.GetFirstOrDefault(v => v.Id == vehicleid);
            if (vehicle == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound();
            }
            return Ok(vehicle);
        }
        /// <summary>
        /// adds a new vehicle
        /// </summary>
        /// <param name="vehicleDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] VehicleDTO vehicleDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            var vehicle = Mapper.Map<Vehicle>(vehicleDTO);  
            await Repository.Add(vehicle);
            await Repository.Save();

            Logger.LogInformation($"Added vehicle with id {vehicle.Id}");
            return CreatedAtAction(nameof(Get), new { vehicleid = vehicle.Id }, vehicle);
        }
        /// <summary>
        /// updates a vehicle
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="vehicleDTO"></param>
        /// <returns></returns>
        [HttpPut("{vehicleid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int vehicleid, [FromBody] VehicleDTO vehicleDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }

            if(!await Repository.CheckVehicleExists(vehicleid))
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound();
            }
            var vehicle = Mapper.Map<Vehicle>(vehicleDTO);


            Repository.UpdateEntity(vehicle);
                await Repository.Save();
            

            Logger.LogInformation($"Updated vehicle with id {vehicle.Id}");
            return NoContent();
        }
        /// <summary>
        /// deletes a vehicle 
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        [HttpDelete("{vehicleid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int vehicleid)
        {
            if (!await Repository.CheckVehicleExists(vehicleid))
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound();
            }
            var vehicle = await Repository.GetFirstOrDefault(v => v.Id == vehicleid);
            Repository.Remove(vehicle);
            await Repository.Save();
            Logger.LogInformation($"Deleted vehicle with id {vehicleid}");
            return NoContent();
        }
    }
}
