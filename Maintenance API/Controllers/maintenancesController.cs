using AutoMapper;
using Maintenance_API.Data;
using Maintenance_API.DTO;
using Maintenance_API.Model;
using Maintenance_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Maintenance_API.Controllers
{
    [Route("api/v{version:apiVersion}/vehicles/{vehicleid}/maintenance")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
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
        /// <summary>
        /// gets all maintenance records for a vehicle
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MaintenanceRecord>), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(int vehicleid)
        {

            var result = await vehicleApiService.GetVehicleById(vehicleid);


            if (result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
            }

            var maintenancerecords = await Repository.Where(d=>d.VehicleId==vehicleid);

            if(maintenancerecords == null)
            {
                Logger.LogInformation($"maintenance records for vehicle with id: {vehicleid} not found");
                return NotFound($"maintenance records for vehicle with id: {vehicleid} not found");
            }   

            Logger.LogInformation($"Retrieved {maintenancerecords.Count()} vehicles");
            return Ok(maintenancerecords);
        }
        /// <summary>
        /// gets a maintenance record by id for a vehicle
        /// </summary>
        /// <param name="maintenanceid"></param>
        /// <returns></returns>
        [HttpGet("{maintenanceid}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(MaintenanceRecord),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int vehicleid, int maintenanceid)
        {
            var result = await vehicleApiService.GetVehicleById(vehicleid);



            if (result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
            }

            var maintenanceRecord = await Repository.GetFirstOrDefault(v => v.Id == maintenanceid && v.VehicleId== vehicleid);
            if (maintenanceRecord == null)
            {
                Logger.LogInformation($"maintenance with id {maintenanceid} for car with id {vehicleid} not found");
                return NotFound();
            }
            return Ok(maintenanceRecord);
        }
        /// <summary>
        /// Assigns a new maintenance record to a vehicle
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <param name="maintenanceRecordDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(MaintenanceRecord),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(int vehicleid,[FromBody] MaintenanceRecordPostDTO maintenanceRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }

            var result =await vehicleApiService.GetVehicleById(vehicleid);

            if(result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
            }


            var maintenanceRecord = Mapper.Map<MaintenanceRecord>(maintenanceRecordDTO);
            maintenanceRecord.VehicleId = vehicleid;
            await Repository.Add(maintenanceRecord);
            await Repository.Save();
            Logger.LogInformation($"Added vehicle with id {maintenanceRecord.Id}");
            return CreatedAtAction(nameof(Get), new { vehicleid=vehicleid, maintenanceid = maintenanceRecord.Id },maintenanceRecord);
        }
        /// <summary>
        /// updates a maintenance record for a vehicle
        /// </summary>
        /// <param name="maintenanceid"></param>
        /// <param name="maintenanceRecordDTO"></param>
        /// <returns></returns>
        [HttpPut("{maintenanceid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int vehicleid,int maintenanceid, [FromBody] MaintenanceRecordDTO maintenanceRecordDTO)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation($"Invalid vehicle data");
                return BadRequest(ModelState);
            }
            var result = await vehicleApiService.GetVehicleById(vehicleid);

            if (result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
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
        /// <summary>
        /// Deletes a maintenance record for a vehicle
        /// </summary>
        /// <param name="maintenanceid"></param>
        /// <returns></returns>
        [HttpDelete("{maintenanceid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int vehicleid, int maintenanceid)
        {
            var result = await vehicleApiService.GetVehicleById(vehicleid);

            if (result == null)
            {
                Logger.LogInformation($"Vehicle with id {vehicleid} not found");
                return NotFound($"Vehicle with id {vehicleid} was not found");
            }


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
