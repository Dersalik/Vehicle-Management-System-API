using AutoMapper;
using Maintenance_API.Controllers;
using Maintenance_API.Data;
using Maintenance_API.DTO;
using Maintenance_API.Model;
using Maintenance_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceAPITests;

    public class MaintenancesControllerTests
    {
        private readonly Mock<IRepository> mockRepository;
        private readonly Mock<ILogger<maintenancesController>> mockLogger;
        private readonly Mock<IVehicleApiService> mockVehicleApiService;
        private readonly Mock<IMapper> mockMapper;
        private readonly maintenancesController controller;

        public MaintenancesControllerTests()
        {
            mockRepository = new Mock<IRepository>();
            mockLogger = new Mock<ILogger<maintenancesController>>();
            mockVehicleApiService = new Mock<IVehicleApiService>();
            mockMapper = new Mock<IMapper>();
            controller = new maintenancesController(mockRepository.Object, mockLogger.Object, mockMapper.Object, mockVehicleApiService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenVehicleExists()
        {
            // Arrange
            var vehicleId = 1;
            var maintenanceRecords = new List<MaintenanceRecord> { new MaintenanceRecord { Id = 1, VehicleId = vehicleId } };
            mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(new VehicleDTO());
            mockRepository.Setup(x => x.Where(d => d.VehicleId == vehicleId)).ReturnsAsync(maintenanceRecords);

            // Act
            var result = await controller.GetAll(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(maintenanceRecords, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = 1;
            VehicleDTO vehicle = null;
            mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);

            // Act
            var result = await controller.GetAll(vehicleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal($"Vehicle with id {vehicleId} was not found", notFoundResult.Value);
        }

    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenMaintenanceRecordsDoNotExist()
    {
        // Arrange
        var vehicleId = 1;
        mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(new VehicleDTO());
        List<MaintenanceRecord> maintenanceRecords = null;
        mockRepository.Setup(x => x.Where(d => d.VehicleId == vehicleId)).ReturnsAsync(maintenanceRecords);

        // Act
        var result = await controller.GetAll(vehicleId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal($"maintenance records for vehicle with id: {vehicleId} not found", notFoundResult.Value);
    }
            [Fact]
            public async Task Get_ReturnsOkResult_WhenMaintenanceRecordExists()
            {
                // Arrange
                var vehicleId = 1;
                var maintenanceId = 1;
                var maintenanceRecord = new MaintenanceRecord { Id = maintenanceId, VehicleId = vehicleId };
                mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(new VehicleDTO());
                mockRepository.Setup(x => x.GetFirstOrDefault(v => v.Id == maintenanceId && v.VehicleId == vehicleId)).ReturnsAsync(maintenanceRecord);

                // Act
                var result = await controller.Get(vehicleId, maintenanceId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.Equal(maintenanceRecord, okResult.Value);
            }

            [Fact]
            public async Task Get_ReturnsNotFound_WhenVehicleDoesNotExist()
            {
                // Arrange
                var vehicleId = 1;
                var maintenanceId = 1;
                VehicleDTO vehicle = null;
                mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);

                // Act
                var result = await controller.Get(vehicleId, maintenanceId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.Equal($"Vehicle with id {vehicleId} was not found", notFoundResult.Value);
            }

            [Fact]
             public async Task Get_ReturnsNotFound_WhenMaintenanceRecordDoesNotExist()
            {
                // Arrange
                var vehicleId = 1;
                var maintenanceId = 1;
                mockVehicleApiService.Setup(x => x.GetVehicleById(vehicleId)).ReturnsAsync(new VehicleDTO());

                MaintenanceRecord maintenanceRecord = null;
                
                mockRepository.Setup(x => x.GetFirstOrDefault(v => v.Id == maintenanceId && v.VehicleId == vehicleId)).ReturnsAsync(maintenanceRecord);

                // Act
                var result = await controller.Get(vehicleId, maintenanceId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            }


    [Fact]
    public async Task Post_WithValidData_ReturnsCreatedResponse()
    {
        // Arrange
        int vehicleId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordPostDTO { Cost=223,Date= DateTime.UtcNow,ServiceType="dfd" };
        var vehicle = new VehicleDTO { Make="test",Model="test"};
        MaintenanceRecord maintenanceRecord = new MaintenanceRecord { Cost = 223, Date = DateTime.UtcNow, ServiceType = "dfd" };
        mockMapper.Setup(x => x.Map<MaintenanceRecord>(It.IsAny<MaintenanceRecordPostDTO>())).Returns(maintenanceRecord);

        mockVehicleApiService.Setup(s => s.GetVehicleById(It.IsAny<int>())).ReturnsAsync(vehicle);
        mockRepository.Setup(r => r.Add(It.IsAny<MaintenanceRecord>())).Callback<MaintenanceRecord>(record =>
        {
            // Simulate saving the record by assigning an Id
            record.Id = 1;
        });

        // Act
        var result = await controller.Post(vehicleId, maintenanceRecordDTO) as CreatedAtActionResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(It.IsAny<int>()), Times.Once);
        mockRepository.Verify(r => r.Add(It.IsAny<MaintenanceRecord>()), Times.Once);
        mockRepository.Verify(r => r.Save(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        Assert.Equal("Get", result.ActionName);
        //Assert.Equal(new { vehicleid = vehicleId, maintenanceid = 1 }, result.RouteValues);
        Assert.IsType<MaintenanceRecord>(result.Value);
    }

    [Fact]
    public async Task Post_WithInvalidVehicleId_ReturnsNotFoundResponse()
    {
        // Arrange
        int vehicleId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordPostDTO { /* Initialize DTO with valid data */ };
        VehicleDTO vehicle = null;

        mockVehicleApiService.Setup(s => s.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);

        // Act
        var result = await controller.Post(vehicleId, maintenanceRecordDTO) as NotFoundObjectResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(vehicleId), Times.Once);
        mockRepository.Verify(r => r.Add(It.IsAny<MaintenanceRecord>()), Times.Never);
        mockRepository.Verify(r => r.Save(), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal($"Vehicle with id {vehicleId} was not found", result.Value);
    }

    [Fact]
    public async Task Post_WithInvalidData_ReturnsBadRequestResponse()
    {
        // Arrange
        int vehicleId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordPostDTO { /* Initialize DTO with invalid data */ };

        var vehicle = new VehicleDTO { Make = "test", Model = "test" };

        mockVehicleApiService.Setup(s => s.GetVehicleById(It.IsAny<int>())).ReturnsAsync(vehicle);
        controller.ModelState.AddModelError("property", "Error message");

        // Act
        var result = await controller.Post(vehicleId, maintenanceRecordDTO) as BadRequestObjectResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(It.IsAny<int>()), Times.Never);
        mockRepository.Verify(r => r.Add(It.IsAny<MaintenanceRecord>()), Times.Never);
        mockRepository.Verify(r => r.Save(), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<SerializableError>(result.Value);
    }

    [Fact]
    public async Task Put_WithValidData_ReturnsNoContentResponse()
    {
        // Arrange
        int vehicleId = 1;
        int maintenanceId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordDTO { Cost = 223, Date = DateTime.UtcNow, ServiceType = "dfd" };
        var vehicle = new VehicleDTO { /* Initialize vehicle object */ };
        MaintenanceRecord maintenanceRecord = new MaintenanceRecord { Cost = 223, Date = DateTime.UtcNow, ServiceType = "dfd" };

        mockMapper.Setup(x => x.Map<MaintenanceRecord>(It.IsAny<MaintenanceRecordDTO>())).Returns(maintenanceRecord);

        mockVehicleApiService.Setup(s => s.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);
        mockRepository.Setup(r => r.CheckRecordExists(maintenanceId)).ReturnsAsync(true);

        // Act
        var result = await controller.Put(vehicleId, maintenanceId, maintenanceRecordDTO) as NoContentResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(vehicleId), Times.Once);
        mockRepository.Verify(r => r.CheckRecordExists(maintenanceId), Times.Once);
        mockRepository.Verify(r => r.UpdateEntity(It.IsAny<MaintenanceRecord>()), Times.Once);
        mockRepository.Verify(r => r.Save(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [Fact]
    public async Task Put_WithInvalidVehicleId_ReturnsNotFoundResponse()
    {
        // Arrange
        int vehicleId = 1;
        int maintenanceId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordDTO { /* Initialize DTO with valid data */ };
        VehicleDTO vehicle = null;

        mockVehicleApiService.Setup(s => s.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);

        // Act
        var result = await controller.Put(vehicleId, maintenanceId, maintenanceRecordDTO) as NotFoundObjectResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(vehicleId), Times.Once);
        mockRepository.Verify(r => r.CheckRecordExists(It.IsAny<int>()), Times.Never);
        mockRepository.Verify(r => r.UpdateEntity(It.IsAny<MaintenanceRecord>()), Times.Never);
        mockRepository.Verify(r => r.Save(), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal($"Vehicle with id {vehicleId} was not found", result.Value);
    }

    [Fact]
    public async Task Put_WithInvalidData_ReturnsBadRequestResponse()
    {
        // Arrange
        int vehicleId = 1;
        int maintenanceId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordDTO { /* Initialize DTO with invalid data */ };

        controller.ModelState.AddModelError("property", "Error message");

        // Act
        var result = await controller.Put(vehicleId, maintenanceId, maintenanceRecordDTO) as BadRequestObjectResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(It.IsAny<int>()), Times.Never);
        mockRepository.Verify(r => r.CheckRecordExists(It.IsAny<int>()), Times.Never);
        mockRepository.Verify(r => r.UpdateEntity(It.IsAny <MaintenanceRecord>()), Times.Never);
        mockRepository.Verify(r => r.Save(), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<SerializableError>(result.Value);
    }
    [Fact]
    public async Task Put_RecordNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        int vehicleId = 1;
        int maintenanceId = 1;
        var maintenanceRecordDTO = new MaintenanceRecordDTO { /* Initialize DTO with valid data */ };
        var vehicle = new VehicleDTO { /* Initialize vehicle object */ };

        mockVehicleApiService.Setup(s => s.GetVehicleById(vehicleId)).ReturnsAsync(vehicle);
        mockRepository.Setup(r => r.CheckRecordExists(maintenanceId)).ReturnsAsync(false);

        // Act
        var result = await controller.Put(vehicleId, maintenanceId, maintenanceRecordDTO) as NotFoundResult;

        // Assert
        mockVehicleApiService.Verify(s => s.GetVehicleById(vehicleId), Times.Once);
        mockRepository.Verify(r => r.CheckRecordExists(maintenanceId), Times.Once);
        mockRepository.Verify(r => r.UpdateEntity(It.IsAny<MaintenanceRecord>()), Times.Never);
        mockRepository.Verify(r => r.Save(), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
    }
}

