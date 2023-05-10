using AutoMapper;
using Maintenance_API.Data;
using Maintenance_API.DTO;
using Maintenance_API.Model;
using Maintenance_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance_API.Controllers
{
    [Route("api/v{version:apiVersion}/vehicles/{id}/maintenance")]
    [ApiVersion("1.0")]
    [ApiController]
    public class maintenanceController : ControllerBase
    {
        IRepository Repository { get; set; }
        ILogger<maintenanceController> Logger;
        IVehicleApiService vehicleApiService { get; set; }
        private IMapper Mapper
        {
            get;
        }
        public maintenanceController(IRepository repository, ILogger<maintenanceController> logger,IMapper mapper, IVehicleApiService vehicleApiService)
        {
            Repository = repository;
            Logger = logger;
            Mapper = mapper;
            this.vehicleApiService = vehicleApiService;
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
        public async Task<IActionResult> Post([FromBody] MaintenanceRecordDTO maintenanceRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            var maintenanceRecord = Mapper.Map<MaintenanceRecord>(maintenanceRecordDTO);
            await Repository.Add(maintenanceRecord);
            await Repository.Save();
            Logger.LogInformation($"Added vehicle with id {maintenanceRecord.Id}");
            return CreatedAtAction(nameof(Get), new { id = maintenanceRecord.Id }, maintenanceRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MaintenanceRecordDTO maintenanceRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }

            if (!await Repository.CheckRecordExists(id))
            {
                Logger.LogInformation($"Vehicle with id {id} not found");
                return NotFound();
            }
            var maintenanceRecord = Mapper.Map<MaintenanceRecord>(maintenanceRecordDTO);    
            maintenanceRecord.Id = id;
            Repository.UpdateEntity(maintenanceRecord);
            await Repository.Save();


            Logger.LogInformation($"Updated vehicle with id {maintenanceRecord.Id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await Repository.CheckRecordExists(id))
            {
                Logger.LogInformation($"Vehicle with id {id} not found");
                return NotFound();
            }
            var maintenanceRecord = await Repository.GetFirstOrDefault(v => v.Id == id);
            Repository.Remove(maintenanceRecord);
            await Repository.Save();
            Logger.LogInformation($"Deleted vehicle with id {id}");
            return NoContent();
        }
    }
}
