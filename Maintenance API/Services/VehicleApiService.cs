using Maintenance_API.DTO;
using Newtonsoft.Json;

namespace Maintenance_API.Services
{
    public class VehicleApiService : IVehicleApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VehicleApiService> _logger;

        public VehicleApiService(HttpClient httpClient, ILogger<VehicleApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<VehicleDTO> GetVehicleById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/vehicles/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve vehicle with id {id}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var vehicle = JsonConvert.DeserializeObject<Vehicle>(content);

                _logger.LogInformation($"Retrieved vehicle with id {id}");
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with id {id}");
                return null;
            }
        }
    }
}
