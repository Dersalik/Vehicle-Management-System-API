using AutoMapper;
using Maintenance_API.Data;
using Maintenance_API.DTO;
using Maintenance_API.Model;
using Maintenance_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance_API.Controllers
{
    [Route("api/v{version:apiVersion}/vehicles/{vehicleid}/maintenance")]
    [ApiVersion("1.0")]
    [ApiController]
    public class maintenancesController : ControllerBase
    {
        IRepository Repository { get; set; }
        ILogger<maintenancesController> Logger;
        IVehicleApiService vehicleApiService { get; set; }
        private IMapper Mapper
        {
            get;
        }
        public maintenancesController(IRepository repository, ILogger<maintenancesController> logger,IMapper mapper, IVehicleApiService vehicleApiService)
        {
            Repository = repository;
            Logger = logger;
            Mapper = mapper;
            this.vehicleApiService = vehicleApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int vehicleid)
        {

            var vehicles = await Repository.Where(d=>d.VehicleId==vehicleid);
            Logger.LogInformation($"Retrieved {vehicles.Count()} vehicles");
            return Ok(vehicles);
        }

        [HttpGet("{maintenanceid}")]
        public async Task<IActionResult> Get(int maintenanceid)
        {
            var vehicle = await Repository.GetFirstOrDefault(v => v.Id == maintenanceid);
            if (vehicle == null)
            {
                Logger.LogInformation($"Vehicle with id {maintenanceid} not found");
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int vehicleid,[FromBody] MaintenanceRecordPostDTO maintenanceRecordDTO)
        {

            var result =await vehicleApiService.GetVehicleById(vehicleid);

            if(result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
            }

            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            var maintenanceRecord = Mapper.Map<MaintenanceRecord>(maintenanceRecordDTO);
            maintenanceRecord.VehicleId = vehicleid;
            await Repository.Add(maintenanceRecord);
            await Repository.Save();
            Logger.LogInformation($"Added vehicle with id {maintenanceRecord.Id}");
            return CreatedAtAction(nameof(Get), new { vehicleid=vehicleid, maintenanceid = maintenanceRecord.Id },maintenanceRecord);
        }

        [HttpPut("{maintenanceid}")]
        public async Task<IActionResult> Put(int maintenanceid, [FromBody] MaintenanceRecordDTO maintenanceRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }

            if (!await Repository.CheckRecordExists(maintenanceid))
            {
                Logger.LogInformation($"Vehicle with id {maintenanceid} not found");
                return NotFound();
            }
            var maintenanceRecord = Mapper.Map<MaintenanceRecord>(maintenanceRecordDTO);    
            maintenanceRecord.Id = maintenanceid;
            Repository.UpdateEntity(maintenanceRecord);
            await Repository.Save();


            Logger.LogInformation($"Updated vehicle with id {maintenanceRecord.Id}");
            return NoContent();
        }

        [HttpDelete("{maintenanceid}")]
        public async Task<IActionResult> Delete(int maintenanceid)
        {
            if (!await Repository.CheckRecordExists(maintenanceid))
            {
                Logger.LogInformation($"Vehicle with id {maintenanceid} not found");
                return NotFound();
            }
            var maintenanceRecord = await Repository.GetFirstOrDefault(v => v.Id == maintenanceid);
            Repository.Remove(maintenanceRecord);
            await Repository.Save();
            Logger.LogInformation($"Deleted vehicle with id {maintenanceid}");
            return NoContent();
        }
    }
}
