using Maintenance_API.DTO;

namespace Maintenance_API.Services
{
    public interface IVehicleApiService
    {
        public Task<VehicleDTO> GetVehicleById(int id);

    }
}